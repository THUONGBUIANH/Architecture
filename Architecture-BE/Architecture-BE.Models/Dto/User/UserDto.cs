using System;

namespace Architecture_BE.Models.Dto
{
    public class UserDto: BaseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
