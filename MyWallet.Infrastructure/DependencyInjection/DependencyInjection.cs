using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyWallet.Application.Common.Context;
using MyWallet.Application.Contracts.IConfigs;
using MyWallet.Application.Contracts.IContext;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Domain.Constants;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Domain.Interface.IRepositories.Base;
using MyWallet.Domain.Interface.IUnitOfWork;
using MyWallet.Infrastructure.Persistence.MyDbContext;
using MyWallet.Infrastructure.Persistence.Repositories;
using MyWallet.Infrastructure.Persistence.UnitOfWork;
using MyWallet.Infrastructure.Security;
using MyWallet.Infrastructure.SubService;
using StackExchange.Redis;
using IDbConnectionFactory = MyWallet.Domain.Interface.IDbContext.IDbConnectionFactory;

namespace MyWallet.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbConnectionFactory();
            services.AddRepo();
            services.AddDatabase(configuration);
            services.AddSubServices();
            services.AddConfig(configuration);
            services.ConfigRedis(configuration);
        }
        private static void AddDbConnectionFactory(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        }
        private static void AddRepo(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register specific repositories
            services.AddScoped<IUserRepository, UserRepository>();  
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IQRHistoryRepository, QRHistoryRepository>();
            services.AddScoped<IBankInfoRepository, BankInfoRepository>();
        }
        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyWalletDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Database.DefaultConnection));
            });
        }
        private static void AddSubServices(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<ITokenService, TokenService>(); 
            services.AddScoped<IIdGenerator, SqlServerIdGenerator>();
        }
        private static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfiguration>(configuration.GetSection("JWT"));
            services.AddScoped<ITokenConfiguration>(sp => sp.GetRequiredService<IOptions<TokenConfiguration>>().Value);
        }
        private static IServiceCollection ConfigRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = configuration[Redis.RedisConnection];

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException(
                        $"Redis connection string '{Redis.RedisConnection}' is not configured.");

                // --- Cấu hình bổ sung ---
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = configuration.GetValue<bool>(Redis.AbortOnConnectFail);

                options.ConnectRetry = configuration.GetValue<int>(Redis.ConnectRetry);

                options.ConnectTimeout = configuration.GetValue<int>(Redis.ConnectTimeoutMs);

                options.SyncTimeout = configuration.GetValue<int>(Redis.SyncTimeoutMs);

                options.ReconnectRetryPolicy = new LinearRetry(configuration.GetValue<int>(Redis.ReconnectRetryIntervalMs));

                return ConnectionMultiplexer.Connect(options);
            });

            return services;
        }

    }
}
