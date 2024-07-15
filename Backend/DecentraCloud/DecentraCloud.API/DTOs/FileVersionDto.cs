namespace DecentraCloud.API.DTOs
{
    public class FileVersionDto
    {
        public string UserId { get; set; }
        public string Filename { get; set; }
        public int VersionNumber { get; set; }
        public string NodeId { get; set; }
    }
}
