namespace DecentraCloud.API.DTOs
{
    public class FileRevokePermissionDto
    {
        public string OwnerId { get; set; }
        public string FileId { get; set; }
        public string UserId { get; set; }
    }
}
