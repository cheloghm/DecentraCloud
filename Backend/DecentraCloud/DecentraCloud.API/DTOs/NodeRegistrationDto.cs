namespace DecentraCloud.API.DTOs
{
    public class NodeRegistrationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Storage { get; set; }
        public string Endpoint { get; set; }
    }
}
