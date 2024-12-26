using MongoDB.Bson.Serialization.Attributes;

namespace BuckshotPlusPlus.Models
{
    public class GitAuth
    {
        [BsonElement("Type")]
        public string Type { get; set; } = "none";

        [BsonElement("Username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("Token")]
        public string Token { get; set; } = string.Empty;

        [BsonElement("SshKey")]
        public string SshKey { get; set; } = string.Empty;

        [BsonElement("SshKeyPassphrase")]
        public string SshKeyPassphrase { get; set; } = string.Empty;
    }
}