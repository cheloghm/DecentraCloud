namespace DecentraCloud.API.DTOs
{
    public class FileMoveDto
    {
        public string UserId { get; set; }
        public string OldFilename { get; set; }
        public string NewFilename { get; set; }
        public byte[] Data { get; set; }
        public string OldNodeId { get; set; }
        public string NewNodeId { get; set; }
    }
}
