using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BuckshotPlusPlus.Models
{
    public class Tenant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("domain")]
        public string Domain { get; set; } = string.Empty;

        [BsonElement("repoUrl")]
        public string RepoUrl { get; set; } = string.Empty;

        [BsonElement("branch")]
        public string Branch { get; set; } = "main";

        [BsonElement("entryFile")]
        public string EntryFile { get; set; } = string.Empty;

        [BsonElement("cacheKey")]
        public string CacheKey { get; set; } = string.Empty;

        [BsonElement("planId")]
        public string PlanId { get; set; } = string.Empty;

        [BsonElement("monthlyPageViewLimit")]
        public int MonthlyPageViewLimit { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("auth")]
        public GitAuth Auth { get; set; } = new GitAuth();

        [BsonElement("stripeCustomerId")]
        public string StripeCustomerId { get; set; } = string.Empty;

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        #nullable enable
        [BsonIgnore]
        public Tokenizer? SiteTokenizer { get; set; }
    }
}