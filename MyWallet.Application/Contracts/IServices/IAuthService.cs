using Microsoft.AspNetCore.Http;
using MyWallet.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IAuthService
    {
        Task<SignInGoogleRes> SignInGoogle(HttpContext httpContext);
        Task<GetUserRes> Me();
    }
}
