using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BuckshotPlusPlus.Models;

public class PageView
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}