using Architecture_BE.Models.Dto;
using System.Threading.Tasks;

namespace Architecture_BE.Business.Services
{
    public interface ITokenService
    {
        Task<RefreshTokenDto> CreateRefreshTokenByUserDtoAsync(UserDto userDto);
        Task<RefreshTokenDto> CheckRefreshTokenAsync(string refreshToken);
    }
}
