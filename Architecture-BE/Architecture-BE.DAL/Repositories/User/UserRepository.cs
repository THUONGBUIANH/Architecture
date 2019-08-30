using Architecture_BE.DAL.DataContext;
using Architecture_BE.DAL.Entities;
using Architecture_BE.Helper.Extensions;
using Architecture_BE.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbArchitectureContext context) : base(context)
        {
        }

        public async Task<User> GetUserActiveById(Guid id)
        {
            return await FindBy(x => x.Id == id && x.IsDeleted == StatusEnum.Active).SingleOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<bool> UserExists(string userName)
        {
            return await AnyAsync(x => x.UserName == userName);
        }

        public async Task<User> VerifyAccount(string userName, string password)
        {
            return await FindBy(x => x.UserName == userName
                                  && x.Password == password.MD5Hash()
                                  && x.IsDeleted == StatusEnum.Active)
                         .SingleOrDefaultAsync();
        }
    }
}
