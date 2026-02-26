using MyWallet.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
