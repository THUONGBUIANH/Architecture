using Architecture_BE.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetUserActiveById(Guid id);
        Task<List<User>> GetUsers();
        Task<User> VerifyAccount(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
