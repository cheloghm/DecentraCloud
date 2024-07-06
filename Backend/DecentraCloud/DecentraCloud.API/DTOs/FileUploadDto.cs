namespace DecentraCloud.API.DTOs
{
    public class FileUploadDto
    {
        public string NodeId { get; set; }
        public string UserId { get; set; }
        public string Filename { get; set; }
        public string Content { get; set; }
    }
}
