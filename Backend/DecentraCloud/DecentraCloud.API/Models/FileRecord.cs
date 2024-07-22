using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DecentraCloud.API.Models
{
    public class FileRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Filename { get; set; }
        public string NodeId { get; set; }
        public long Size { get; set; }
    }
}
