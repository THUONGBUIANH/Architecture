using System;
using System.Threading.Tasks;
using Architecture_BE.DAL.Entities;
using Architecture_BE.DAL.UnitOfWork;
using Architecture_BE.Models.Dto;

namespace Architecture_BE.Business.Services
{
    public class TokenService : ServiceBase, ITokenService
    {
        public TokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<RefreshTokenDto> CheckRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _unitOfWork.TokenRepository.GetTokenByRefreshTokenAsync(refreshToken);

            if (tokenEntity == null || tokenEntity.ExpiresUtc < DateTime.UtcNow)
            {
                return null;
            }
              
            return new RefreshTokenDto
            {
                UserId = tokenEntity.UserId,
                RefreshToken = tokenEntity.RefreshToken
            };
        }

        public async Task<RefreshTokenDto> CreateRefreshTokenByUserDtoAsync(UserDto userDto)
        {
            var tokenEntity = await _unitOfWork.TokenRepository.GetTokenByUserIdAsync(userDto.Id);

            if (tokenEntity != null)
            {
                _unitOfWork.TokenRepository.RemoveToken(tokenEntity);
                await _unitOfWork.CommitAsync();
            }

            var newToken = new Token
            {
                UserId = userDto.Id,
                RefreshToken = Guid.NewGuid().ToString("N"),
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            _unitOfWork.TokenRepository.Create(newToken);
            await _unitOfWork.CommitAsync();

            return new RefreshTokenDto
            {
                UserId = newToken.UserId,
                RefreshToken = newToken.RefreshToken
            };
        }
    }
}
