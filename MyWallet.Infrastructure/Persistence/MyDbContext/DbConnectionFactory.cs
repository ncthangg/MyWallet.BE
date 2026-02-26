using Microsoft.Extensions.Configuration;
using MyWallet.Domain.Constants;
using MyWallet.Domain.Interface.IDbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Infrastructure.Persistence.MyDbContext
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString(Database.DefaultConnection));
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var conn = new SqlConnection(_configuration.GetConnectionString(Database.DefaultConnection));
            await conn.OpenAsync();
            return conn;
        }
    }

}
