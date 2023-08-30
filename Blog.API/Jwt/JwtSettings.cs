namespace Blog.API.Core
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public int Minutes { get; set; }
    }
}
