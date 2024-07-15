using MongoDB.Bson.Serialization.Attributes;

namespace DecentraCloud.API.Models
{
    public class FilePermission
    {
        [BsonId]
        public string Id { get; set; }
        public string FileId { get; set; }
        public string OwnerId { get; set; }
        public string UserId { get; set; } // The user to whom the permission is granted
        public string PermissionType { get; set; } // e.g., "view", "edit"
    }
}
