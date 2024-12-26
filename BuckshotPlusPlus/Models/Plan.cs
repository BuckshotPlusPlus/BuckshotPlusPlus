using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuckshotPlusPlus.Models;

public class Plan
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StripeProductId { get; set; } = string.Empty;
    public string StripePriceId { get; set; } = string.Empty;
    public int MonthlyPageViews { get; set; }
    public decimal MonthlyPrice { get; set; }
    public decimal OverageRate { get; set; }
}