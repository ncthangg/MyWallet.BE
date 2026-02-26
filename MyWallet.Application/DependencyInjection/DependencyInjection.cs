using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyWallet.Application.Common;
using MyWallet.Application.Common.Context;
using MyWallet.Application.Contracts.IConfigs;
using MyWallet.Application.Contracts.IContext;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.Services;
using MyWallet.Domain.Constants;
using StackExchange.Redis;

namespace MyWallet.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddService();
        }
        private static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IQRHistoryService, QRHistoryService>();
            services.AddScoped<IBankInfoService, BankInfoService>();
        }
    }
}
