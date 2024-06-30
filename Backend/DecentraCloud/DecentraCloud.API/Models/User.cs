using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DecentraCloud.API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
        public UserSettings Settings { get; set; }
    }
}
