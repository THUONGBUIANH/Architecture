using System;
using System.Threading.Tasks;
using Architecture_BE.DAL.DataContext;
using Architecture_BE.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Architecture_BE.DAL.Repositories
{
    public class TokenRepository : GenericRepository<Token>, ITokenRepository
    {
        public TokenRepository(DbArchitectureContext context) : base(context)
        {
        }

        public async Task<Token> GetTokenByRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await FindBy(x => x.RefreshToken.Equals(refreshToken))
                                   .SingleOrDefaultAsync();

            return tokenEntity;
        }

        public async Task<Token> GetTokenByUserIdAsync(Guid userId)
        {
            var tokenEntity = await FindBy(x => x.UserId == userId)
                                   .SingleOrDefaultAsync();

            return tokenEntity;
        }

        public void RemoveToken(Token tokenEntity)
        {
            _context.Entry<Token>(tokenEntity).State = EntityState.Deleted;
        }
    }
}
