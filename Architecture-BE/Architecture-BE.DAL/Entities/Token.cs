using System;

namespace Architecture_BE.DAL.Entities
{
    public class Token : EntityBase
    {
        public string RefreshToken { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
