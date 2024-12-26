using System.Collections.Concurrent;
using BuckshotPlusPlus.Models;
using BuckshotPlusPlus.Services;
using Serilog;
using BuckshotPlusPlus.WebServer.Extensions;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using System;

namespace BuckshotPlusPlus.WebServer
{
    public class TenantManager
    {
        private readonly ConcurrentDictionary<string, Tenant> _tenantCache;
        private readonly MongoDbService _mongoDb;
        private readonly StripeService _stripe;
        private readonly string _sitesBasePath;
        private readonly ILogger _logger;

        public TenantManager(MongoDbService mongoDb, StripeService stripe, ILogger logger)
        {
            _mongoDb = mongoDb;
            _stripe = stripe;
            _logger = logger;
            _tenantCache = new ConcurrentDictionary<string, Tenant>();
            _sitesBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites");
            Directory.CreateDirectory(_sitesBasePath);
            StartCacheCleanupTimer();
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

                // Track page view
                await TrackPageView(tenant, request);

                // Check if we need to update the site
                var siteDir = Path.Combine(_sitesBasePath, tenant.CacheKey);
                bool needsUpdate = !Directory.Exists(siteDir) ||
                                 (DateTime.UtcNow - tenant.LastUpdated).TotalMinutes > 5;

                if (needsUpdate)
                {
                    _logger.Information("Updating site for domain: {Domain}", domain);
                    await UpdateSite(tenant, siteDir);
                }

                // Return cached tokenizer if available and up to date
                if (tenant.SiteTokenizer != null && !needsUpdate)
                {
                    return tenant.SiteTokenizer;
                }

                // Create new tokenizer
                var entryFile = Path.Combine(siteDir, tenant.EntryFile);
                if (!File.Exists(entryFile))
                {
                    _logger.Error("Entry file not found: {EntryFile} for domain: {Domain}", entryFile, domain);
                    return null;
                }

                _logger.Information("Creating new tokenizer for domain: {Domain}", domain);
                tenant.SiteTokenizer = new Tokenizer(entryFile);
                tenant.LastUpdated = DateTime.UtcNow;

                var update = Builders<Tenant>.Update
                    .Set(t => t.LastUpdated, tenant.LastUpdated);
                await _mongoDb._tenants.UpdateOneAsync(t => t.Id == tenant.Id, update);

                return tenant.SiteTokenizer;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting tenant tokenizer for domain: {Domain}", domain);
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

        private async Task UpdateSite(Tenant tenant, string siteDir)
        {
            try
            {
                _logger.Information("Starting repository update for: {Domain}", tenant.Domain);
                await GitService.UpdateRepository(tenant, siteDir);

                // Update the timestamp
                tenant.LastUpdated = DateTime.UtcNow;

                // Use filters and update definitions explicitly
                var filter = Builders<Tenant>.Filter.Eq(t => t.Id, tenant.Id);
                var update = Builders<Tenant>.Update
                    .Set("lastUpdated", tenant.LastUpdated);

                await _mongoDb._tenants.UpdateOneAsync(filter, update);
                _logger.Information("Successfully updated site for: {Domain}", tenant.Domain);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating site for tenant: {Domain}", tenant.Domain);
                throw;
            }
        }

        private void StartCacheCleanupTimer()
        {
            var timer = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            timer.Elapsed += async (s, e) =>
            {
                foreach (var tenant in _tenantCache)
                {
                    if ((DateTime.UtcNow - tenant.Value.LastUpdated).TotalHours > 24)
                    {
                        _tenantCache.TryRemove(tenant.Key, out _);
                        _logger.Information("Removed expired cache entry for domain: {Domain}", tenant.Key);
                    }
                }
            };
            timer.Start();
        }
    }
}