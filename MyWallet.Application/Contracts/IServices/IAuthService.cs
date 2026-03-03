using Microsoft.AspNetCore.Http;
using MyWallet.Application.DTOs.Response;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IAuthService
    {
        Task<SignInGoogleRes> SignInGoogle(HttpContext httpContext);
        Task<GetUserRes> Me();
    }
}
