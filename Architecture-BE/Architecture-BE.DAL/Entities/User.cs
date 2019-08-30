using System;

namespace Architecture_BE.DAL.Entities
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        //public string Avartar { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
