
using Architecture_BE.DAL.UnitOfWork;
using Architecture_BE.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture_BE.Business.Services
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<bool> Active(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetUserActiveById(id);

            if (user == null)
                return false;

            _unitOfWork.UserRepository.Delete(user);

            return await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Edit(Guid id, EditUserDto editUserDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserActiveById(id);

            if (user == null)
                return false;

            user = Mapping.MappingUserByEditUserDto(user, editUserDto);

            _unitOfWork.UserRepository.Update(user);

            return await _unitOfWork.CommitAsync();
        }

        public async  Task<UserDto> GetUserActiveById(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetUserActiveById(id);

            if (user == null)
                return null;

            return Mapping.MappingUserDtoByUser(user);
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var userDtoList = new List<UserDto>();
            var users = await _unitOfWork.UserRepository.GetUsers();

            if(users != null)
            {
                userDtoList = users.Select(x => Mapping.MappingUserDtoByUser(x)).ToList();
            }

            return userDtoList;
        }

        public async Task<bool> Register(RegisterUserDto registerUserDto)
        {
            var user = Mapping.MappingUserByRegisterUserDto(registerUserDto);

            _unitOfWork.UserRepository.Create(user);

            return await _unitOfWork.CommitAsync();
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _unitOfWork.UserRepository.UserExists(userName);
        }

        public async Task<UserDto> VerifyAccount(LoginUserDto loginUserDto)
        {
            var user = await _unitOfWork.UserRepository.VerifyAccount(loginUserDto.UserName, loginUserDto.Password);

            if (user == null)
                return null;

            return Mapping.MappingUserDtoByUser(user);
        }
    }
}
