using Architecture_BE.DAL.Entities;
using Architecture_BE.Helper.Extensions;
using Architecture_BE.Models.Dto;

namespace Architecture_BE.Business
{
    public partial class Mapping
    {
        public static User MappingUserByRegisterUserDto(RegisterUserDto registerUserDto)
        {
            return new User
            {
                UserName = registerUserDto.UserName,
                Password = registerUserDto.Password.MD5Hash(),
                Email = registerUserDto.Email
            };
        }
        public static UserDto MappingUserDtoByUser(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                BirthDay = user.BirthDay,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                IsDeleted = user.IsDeleted
            };
        }
        public static User MappingUserByEditUserDto(User user, EditUserDto editUserDto)
        {
            user.Email = editUserDto.Email;
            user.BirthDay = editUserDto.BirthDay;

            return user;
        }
    }
}
