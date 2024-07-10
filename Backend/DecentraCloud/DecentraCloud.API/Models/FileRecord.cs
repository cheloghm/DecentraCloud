namespace DecentraCloud.API.Models
{
    public class FileRecord
    {
        public string UserId { get; set; }
        public string Filename { get; set; }
        public string NodeId { get; set; }
        public long Size { get; set; }
    }
}
