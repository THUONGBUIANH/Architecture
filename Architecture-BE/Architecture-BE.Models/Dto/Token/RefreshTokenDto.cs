using System;

namespace Architecture_BE.Models.Dto
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
    }
}
