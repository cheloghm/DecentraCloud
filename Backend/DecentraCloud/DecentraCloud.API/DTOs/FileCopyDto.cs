namespace DecentraCloud.API.DTOs
{
    public class FileCopyDto
    {
        public string UserId { get; set; }
        public string Filename { get; set; }
        public string NewFilename { get; set; }
        public string NodeId { get; set; }
        public string NewNodeId { get; set; }
    }
}
