using Microsoft.Extensions.DependencyInjection;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Services;

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
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IQRHistoryService, QRHistoryService>();
            services.AddScoped<IBankInfoService, BankInfoService>();
        }
    }
}
