using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;
using IDbConnectionFactory = MyWallet.Domain.Interface.IDbContext.IDbConnectionFactory;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory, "Users")
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            const string sql = @"
                SELECT 
                    Id, Email, FullName, GoogleId, SecurityStamp, AvatarUrl, CreatedAt, UpdatedAt
                FROM Users
                WHERE Email = @Email
            ";

            return await QuerySingleAsync<User>(sql, new { Email = email });
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            const string sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

            var count = await QuerySingleAsync<int>(sql, new { Email = email });
            return count > 0;
        }

        public async Task<User> GetWithAccountsAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(id));

            const string sql = "SELECT * FROM Users WHERE Id = @UserId";

            return await QuerySingleAsync<User>(sql, new { UserId = id });
        }
    }
}
