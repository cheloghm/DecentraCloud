namespace DecentraCloud.API.DTOs
{
    public class FileShareDto
    {
        public string OwnerId { get; set; }
        public string Filename { get; set; }
        public string Email { get; set; }
        public string PermissionType { get; set; } // e.g., "view", "edit"
    }
}
