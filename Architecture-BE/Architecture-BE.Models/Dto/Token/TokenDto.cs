using System;

namespace Architecture_BE.Models.Dto
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public DateTime Expiration { get; set; }
    }
}
