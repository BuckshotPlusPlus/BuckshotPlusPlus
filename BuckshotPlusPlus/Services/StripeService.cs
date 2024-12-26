using Stripe;
using BuckshotPlusPlus.Models;
using Serilog;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BuckshotPlusPlus.Services
{
    public class StripeService
    {
        private readonly MongoDbService _mongoDb;
        private readonly ILogger _logger;

        public StripeService(string apiKey, MongoDbService mongoDb)
        {
            StripeConfiguration.ApiKey = apiKey;
            _mongoDb = mongoDb;
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/stripe.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public async Task CreateSubscription(Tenant tenant, string paymentMethodId)
        {
            try
            {
                var customerOptions = new CustomerCreateOptions
                {
                    PaymentMethod = paymentMethodId,
                    Email = $"admin@{tenant.Domain}",
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "tenantId", tenant.Id.ToString() }
                    }
                };

                var customerService = new CustomerService();
                var customer = await customerService.CreateAsync(customerOptions);

                var subscriptionOptions = new SubscriptionCreateOptions
                {
                    Customer = customer.Id,
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = tenant.PlanId,
                        },
                    },
                    PaymentSettings = new SubscriptionPaymentSettingsOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                    }
                };

                var subscriptionService = new SubscriptionService();
                await subscriptionService.CreateAsync(subscriptionOptions);

                // Update tenant with Stripe customer ID
                var filter = Builders<Tenant>.Filter.Eq(t => t.Id, tenant.Id);
                var update = Builders<Tenant>.Update
                    .Set(t => t.StripeCustomerId, customer.Id);

                await _mongoDb._tenants.UpdateOneAsync(filter, update);

                _logger.Information("Created Stripe subscription for tenant: {Domain}", tenant.Domain);
            }
            catch (StripeException ex)
            {
                _logger.Error(ex, "Stripe error for tenant: {Domain}", tenant.Domain);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating subscription for tenant: {Domain}", tenant.Domain);
                throw;
            }
        }

        public async Task ProcessOverageCharges(ObjectId tenantId)
        {
            try
            {
                var tenant = await _mongoDb.GetTenantById(tenantId);
                if (tenant == null || string.IsNullOrEmpty(tenant.StripeCustomerId))
                {
                    _logger.Warning("Cannot process overage charges - tenant not found or no Stripe customer: {Id}", tenantId);
                    return;
                }

                var pageViews = await _mongoDb.GetMonthlyPageViews(tenantId.ToString());
                if (pageViews <= tenant.MonthlyPageViewLimit)
                {
                    return;
                }

                var overage = pageViews - tenant.MonthlyPageViewLimit;
                var overageRate = 0.001m; // $0.001 per extra page view
                var amount = (long)(overage * overageRate * 100); // Convert to cents

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Customer = tenant.StripeCustomerId,
                    Description = $"Overage charges for {overage:N0} page views",
                    Metadata = new Dictionary<string, string>
                    {
                        { "tenantId", tenantId.ToString() },
                        { "pageViews", pageViews.ToString() },
                        { "overage", overage.ToString() }
                    }
                };

                var chargeService = new ChargeService();
                await chargeService.CreateAsync(chargeOptions);

                _logger.Information("Processed overage charges for tenant: {Domain}, Overage: {Overage}",
                    tenant.Domain, overage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing overage charges for tenant: {Id}", tenantId);
                throw;
            }
        }

        private async Task UpdateTenantStripeId(Tenant tenant, string stripeCustomerId)
        {
            tenant.StripeCustomerId = stripeCustomerId;
            await _mongoDb.UpdateTenant(tenant);
        }
    }
}