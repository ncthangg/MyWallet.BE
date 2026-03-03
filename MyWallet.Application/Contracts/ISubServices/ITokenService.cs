using MyWallet.Application.DTOs.Response;
using System.Security.Claims;

namespace MyWallet.Application.Contracts.ISubServices
{
    public interface ITokenService
    {
        Task<TokenRes> GenerateTokens(Guid userId, DateTime? expiredTime);
        Task<TokenRes> GenerateNewRefreshTokenAsync(string oldRefreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenExpired(string token);
    }
}
