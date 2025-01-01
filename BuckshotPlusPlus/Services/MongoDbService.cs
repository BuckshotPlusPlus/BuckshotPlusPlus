using MongoDB.Driver;
using BuckshotPlusPlus.Models;
using Serilog;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace BuckshotPlusPlus.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        internal readonly IMongoCollection<Tenant> _tenants;
        private readonly IMongoCollection<PageView> _pageViews;
        private readonly ILogger _logger;

        public MongoDbService(string connectionString)
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/mongodb.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase("BuckshotPlusPlus");
                _tenants = _database.GetCollection<Tenant>("tenants");
                _pageViews = _database.GetCollection<PageView>("pageViews");

                _logger.Information("MongoDB connection established");
                CreateIndexes();
                LogDatabaseStats().Wait();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to initialize MongoDB connection");
                throw;
            }
        }

        private void CreateIndexes()
        {
            try
            {
                var domainIndex = Builders<Tenant>.IndexKeys.Ascending(t => t.Domain);
                _tenants.Indexes.CreateOne(new CreateIndexModel<Tenant>(
                    domainIndex,
                    new CreateIndexOptions { Unique = true }
                ));

                var pageViewIndexes = Builders<PageView>.IndexKeys
                    .Ascending(p => p.TenantId)
                    .Ascending(p => p.Timestamp);
                _pageViews.Indexes.CreateOne(new CreateIndexModel<PageView>(pageViewIndexes));

                _logger.Information("MongoDB indexes created successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create MongoDB indexes");
                // Don't throw - indexes might already exist
            }
        }

        private async Task LogDatabaseStats()
        {
            try
            {
                var tenantsCount = await _tenants.CountDocumentsAsync(FilterDefinition<Tenant>.Empty);
                var pageViewsCount = await _pageViews.CountDocumentsAsync(FilterDefinition<PageView>.Empty);

                _logger.Information(
                    "Database stats - Tenants: {TenantsCount}, PageViews: {PageViewsCount}",
                    tenantsCount,
                    pageViewsCount
                );

                var tenants = await GetAllTenants();
                var domains = string.Join(", ", tenants.Select(t => t.Domain));
                _logger.Information("Configured domains: {Domains}", domains);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to log database stats");
            }
        }

        #nullable enable
        public async Task<Tenant?> GetTenantByDomain(string domain)
        {
            try
            {
                _logger.Debug("Looking up tenant for domain: {Domain}", domain);

                var filter = Builders<Tenant>.Filter.Regex(
                    t => t.Domain,
                    new BsonRegularExpression($"^{domain}$", "i")
                ) & Builders<Tenant>.Filter.Eq(t => t.IsActive, true);

                var tenant = await _tenants.Find(filter).FirstOrDefaultAsync();

                if (tenant == null)
                {
                    _logger.Warning("No tenant found for domain: {Domain}", domain);
                    return null;
                }

                _logger.Information("Found tenant {Id} for domain {Domain}", tenant.Id, domain);
                return tenant;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving tenant for domain: {Domain}", domain);
                throw;
            }
        }

        public async Task<Tenant?> GetTenantById(ObjectId id)
        {
            try
            {
                return await _tenants.Find(t => t.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving tenant by ID: {Id}", id);
                throw;
            }
        }
        #nullable disable

        public async Task TrackPageView(PageView pageView)
        {
            try
            {
                await _pageViews.InsertOneAsync(pageView);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error tracking page view");
                throw;
            }
        }

        public async Task<int> GetMonthlyPageViews(string tenantId)
        {
            try
            {
                var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                var filter = Builders<PageView>.Filter.And(
                    Builders<PageView>.Filter.Eq(p => p.TenantId, tenantId),
                    Builders<PageView>.Filter.Gte(p => p.Timestamp, startOfMonth)
                );

                return (int)await _pageViews.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting monthly page views for tenant: {TenantId}", tenantId);
                throw;
            }
        }

        public async Task<List<Tenant>> GetAllTenants()
        {
            try
            {
                return await _tenants.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving all tenants");
                throw;
            }
        }

        public async Task UpdateTenantLastUpdated(ObjectId id, DateTime lastUpdated)
        {
            try
            {
                var update = Builders<Tenant>.Update
                    .Set(t => t.LastUpdated, lastUpdated);

                await _tenants.UpdateOneAsync(t => t.Id == id, update);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating tenant last updated time: {Id}", id);
                throw;
            }
        }

        public async Task UpdateTenant(Tenant tenant)
        {
            try
            {
                var filter = Builders<Tenant>.Filter.Eq(t => t.Id, tenant.Id);
                await _tenants.ReplaceOneAsync(filter, tenant);
                _logger.Information("Updated tenant: {Domain}", tenant.Domain);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating tenant: {Domain}", tenant.Domain);
                throw;
            }
        }

        public string GetTenantIdAsString(ObjectId id)
        {
            return id.ToString();
        }

        public ObjectId ConvertToObjectId(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                return objectId;
            }
            throw new ArgumentException($"Invalid ObjectId format: {id}");
        }
    }
}