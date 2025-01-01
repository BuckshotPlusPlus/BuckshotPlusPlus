using System.Collections.Concurrent;
using BuckshotPlusPlus.Models;
using BuckshotPlusPlus.Services;
using Serilog;
using BuckshotPlusPlus.WebServer.Extensions;
using System.Net;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using System;
using LibGit2Sharp;
using System.Threading;

namespace BuckshotPlusPlus.WebServer
{
    public class TenantManager
    {
        private readonly ConcurrentDictionary<string, Tenant> _tenantCache;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _updateLocks;
        private readonly ConcurrentDictionary<string, DateTime> _lastGitChecks;
        private readonly MongoDbService _mongoDb;
        private readonly StripeService _stripe;
        private readonly string _sitesBasePath;
        private readonly ILogger _logger;
        private readonly TimeSpan _gitCheckInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(24);

        public TenantManager(MongoDbService mongoDb, StripeService stripe, ILogger logger)
        {
            _mongoDb = mongoDb;
            _stripe = stripe;
            _logger = logger;
            _tenantCache = new ConcurrentDictionary<string, Tenant>();
            _updateLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
            _lastGitChecks = new ConcurrentDictionary<string, DateTime>();
            _sitesBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites");
            Directory.CreateDirectory(_sitesBasePath);
            StartCacheCleanupTimer();
        }

        private SemaphoreSlim GetUpdateLock(string domain)
        {
            return _updateLocks.GetOrAdd(domain, _ => new SemaphoreSlim(1, 1));
        }

        public async Task<Tokenizer> GetTenantTokenizer(string domain, HttpListenerRequest request)
        {
            try
            {
                // Get tenant from cache or database
                var tenant = await _tenantCache.GetOrAddAsync(domain, async (d) =>
                {
                    try
                    {
                        var t = await _mongoDb.GetTenantByDomain(d);
                        if (t != null)
                        {
                            _logger.Information("Found tenant configuration for domain: {Domain}", d);
                            t.LastUpdated = DateTime.MinValue; // Force update on first load
                            return t;
                        }
                        _logger.Warning("No tenant configuration found for domain: {Domain}", d);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error retrieving tenant for domain: {Domain}", d);
                        return null;
                    }
                });

                if (tenant == null)
                {
                    _logger.Warning("No tenant found for domain: {Domain}", domain);
                    return null;
                }

                // Track page view asynchronously
                _ = TrackPageView(tenant, request);

                var siteDir = Path.Combine(_sitesBasePath, tenant.CacheKey);
                var updateLock = GetUpdateLock(domain);

                // Check if site needs initial setup
                if (!Directory.Exists(siteDir))
                {
                    await updateLock.WaitAsync();
                    try
                    {
                        // Double-check after acquiring lock
                        if (!Directory.Exists(siteDir))
                        {
                            await InitializeSite(tenant, siteDir);
                        }
                    }
                    finally
                    {
                        updateLock.Release();
                    }
                }

                // Check if we need to check for git updates
                bool shouldCheckGit = ShouldCheckGitUpdates(domain);
                if (shouldCheckGit)
                {
                    // Start git check asynchronously
                    _ = CheckGitUpdates(tenant, siteDir, domain);
                }

                // Return cached tokenizer if available
                if (tenant.SiteTokenizer != null)
                {
                    return tenant.SiteTokenizer;
                }

                // Initialize tokenizer if not available
                await updateLock.WaitAsync();
                try
                {
                    // Double-check after acquiring lock
                    if (tenant.SiteTokenizer == null)
                    {
                        var entryFile = Path.Combine(siteDir, tenant.EntryFile);
                        if (!File.Exists(entryFile))
                        {
                            _logger.Error("Entry file not found: {EntryFile} for domain: {Domain}", entryFile, domain);
                            return null;
                        }

                        _logger.Information("Creating new tokenizer for domain: {Domain}", domain);
                        tenant.SiteTokenizer = new Tokenizer(entryFile);
                    }
                    return tenant.SiteTokenizer;
                }
                finally
                {
                    updateLock.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting tenant tokenizer for domain: {Domain}", domain);
                throw;
            }
        }

        private bool ShouldCheckGitUpdates(string domain)
        {
            if (_lastGitChecks.TryGetValue(domain, out var lastCheck))
            {
                return (DateTime.UtcNow - lastCheck) >= _gitCheckInterval;
            }
            return true;
        }

        private async Task InitializeSite(Tenant tenant, string siteDir)
        {
            try
            {
                _logger.Information("Initializing site for domain: {Domain}", tenant.Domain);
                await GitService.UpdateRepository(tenant, siteDir);
                tenant.LastUpdated = DateTime.UtcNow;
                await UpdateTenantLastUpdated(tenant);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error initializing site for domain: {Domain}", tenant.Domain);
                throw;
            }
        }

        private async Task CheckGitUpdates(Tenant tenant, string siteDir, string domain)
        {
            var updateLock = GetUpdateLock(domain);
            await updateLock.WaitAsync();
            try
            {
                var currentHash = GetCurrentCommitHash(siteDir);
                var latestHash = GetLatestCommitHash(tenant);

                if (currentHash != latestHash)
                {
                    _logger.Information("Updates found for domain: {Domain}", domain);
                    await GitService.UpdateRepository(tenant, siteDir);
                    tenant.LastUpdated = DateTime.UtcNow;
                    tenant.SiteTokenizer = null; // Force tokenizer rebuild
                    await UpdateTenantLastUpdated(tenant);
                }

                _lastGitChecks[domain] = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking git updates for domain: {Domain}", domain);
            }
            finally
            {
                updateLock.Release();
            }
        }

        private string GetCurrentCommitHash(string repoPath)
        {
            try
            {
                if (!Directory.Exists(repoPath))
                    return string.Empty;

                using (var repo = new Repository(repoPath))
                {
                    return repo.Head.Tip.Sha;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting current commit hash for repo: {Path}", repoPath);
                return string.Empty;
            }
        }

        private string GetLatestCommitHash(Tenant tenant)
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                var cloneOptions = new CloneOptions
                {
                    Checkout = false,
                    IsBare = true,
                    CredentialsProvider = (_url, _user, _cred) => GetCredentials(tenant.Auth)
                };

                Repository.Clone(tenant.RepoUrl, tempPath, cloneOptions);

                using (var repo = new Repository(tempPath))
                {
                    var remoteBranch = repo.Branches[$"origin/{tenant.Branch}"];
                    return remoteBranch.Tip.Sha;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting latest commit hash for tenant: {Domain}", tenant.Domain);
                return string.Empty;
            }
        }

        private Credentials GetCredentials(GitAuth auth)
        {
            switch (auth.Type?.ToLower())
            {
                case "pat":
                    return new UsernamePasswordCredentials
                    {
                        Username = auth.Username ?? "git",
                        Password = auth.Token
                    };
                case "basic":
                    return new UsernamePasswordCredentials
                    {
                        Username = auth.Username,
                        Password = auth.Token
                    };
                default:
                    return null;
            }
        }

        private async Task UpdateTenantLastUpdated(Tenant tenant)
        {
            try
            {
                var filter = Builders<Tenant>.Filter.Eq(t => t.Id, tenant.Id);
                var update = Builders<Tenant>.Update.Set(t => t.LastUpdated, tenant.LastUpdated);
                await _mongoDb._tenants.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating last updated time for tenant: {Domain}", tenant.Domain);
                throw;
            }
        }

        private async Task TrackPageView(Tenant tenant, HttpListenerRequest request)
        {
            try
            {
                var pageView = new PageView
                {
                    TenantId = tenant.Id.ToString(),
                    Path = request.Url.PathAndQuery,
                    UserAgent = request.UserAgent ?? "Unknown",
                    IpAddress = request.RemoteEndPoint.Address.ToString(),
                    Timestamp = DateTime.UtcNow,
                    SessionId = request.Cookies["bpp_session_id"]?.Value ?? Guid.NewGuid().ToString(),
                    Country = request.Headers["CF-IPCountry"] ?? "Unknown",
                    Region = request.Headers["CF-Region"] ?? "Unknown"
                };

                await _mongoDb.TrackPageView(pageView);

                // Check if we need to process overage charges
                var monthlyViews = await _mongoDb.GetMonthlyPageViews(tenant.Id.ToString());
                if (monthlyViews > tenant.MonthlyPageViewLimit)
                {
                    try
                    {
                        await _stripe.ProcessOverageCharges(tenant.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error processing overage charges for tenant: {Domain}", tenant.Domain);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error tracking page view for tenant: {Domain}", tenant.Domain);
                // Don't throw - we don't want to break the site over analytics
            }
        }

        private void StartCacheCleanupTimer()
        {
            var timer = new System.Timers.Timer(_cacheExpiration.TotalMilliseconds / 2);
            timer.Elapsed += (s, e) =>
            {
                try
                {
                    foreach (var tenant in _tenantCache)
                    {
                        if ((DateTime.UtcNow - tenant.Value.LastUpdated) >= _cacheExpiration)
                        {
                            if (_tenantCache.TryRemove(tenant.Key, out _))
                            {
                                _updateLocks.TryRemove(tenant.Key, out var lockObj);
                                lockObj?.Dispose();
                                _lastGitChecks.TryRemove(tenant.Key, out _);
                                _logger.Information("Removed expired cache entry for domain: {Domain}", tenant.Key);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error during cache cleanup");
                }
            };
            timer.Start();
        }
    }
}