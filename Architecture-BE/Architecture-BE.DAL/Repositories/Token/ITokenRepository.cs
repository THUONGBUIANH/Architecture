using Architecture_BE.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.Repositories
{
    public interface ITokenRepository : IGenericRepository<Token>
    {
        Task<Token> GetTokenByUserIdAsync(Guid userId);
        void RemoveToken(Token tokenEntity);
        Task<Token> GetTokenByRefreshTokenAsync(string refreshToken);
    }
}
