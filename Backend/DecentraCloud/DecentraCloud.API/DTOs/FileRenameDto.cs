namespace DecentraCloud.API.DTOs
{
    public class FileRenameDto
    {
        public string NodeId { get; set; }
        public string UserId { get; set; }
        public string OldFilename { get; set; }
        public string NewFilename { get; set; }
    }
}
