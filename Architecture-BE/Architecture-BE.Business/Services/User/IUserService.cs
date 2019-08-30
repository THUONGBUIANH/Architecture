using Architecture_BE.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture_BE.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> VerifyAccount(LoginUserDto loginUserDto);
        Task<bool> UserExists(string userName);
        Task<bool> Register(RegisterUserDto registerUserDto);
        Task<bool> Active(Guid id);
        Task<bool> Edit(Guid id, EditUserDto editUserDto);
        Task<List<UserDto>> GetUsers();
        Task<UserDto> GetUserActiveById(Guid id);
    }
}
