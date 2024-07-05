using DecentraCloud.API.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.Data.SqlTypes;

namespace DecentraCloud.API.Models
{
    public class Node : IEntityWithId
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("storage")]
        public int Storage { get; set; }

        [BsonElement("uptime")]
        public long Uptime { get; set; }

        [BsonElement("downtime")]
        public long Downtime { get; set; }

        [BsonElement("storageStats")]
        public StorageStats StorageStats { get; set; }

        [BsonElement("onlineStatus")]
        public string OnlineStatus { get; set; }

        [BsonElement("causeOfDowntime")]
        public string CauseOfDowntime { get; set; }
        public string Token { get; set; }
    }

}
