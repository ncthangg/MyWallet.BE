using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;
using IDbConnectionFactory = MyWallet.Domain.Interface.IDbContext.IDbConnectionFactory;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory, "Accounts") 
        {
            
        }

        public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            const string sql = @"
                SELECT 
                    Id, UserId, AccountNumber, AccountHolder, 
                    BankCode, BankName, AccountType, Balance,
                    IsActive, CreatedAt, UpdatedAt
                FROM Accounts
                WHERE UserId = @UserId AND IsActive = 1
                ORDER BY CreatedAt DESC
            ";

            return await QueryAsync<Account>(sql, new { UserId = userId });
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty", nameof(accountNumber));

            const string sql = "SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber";

            return await QuerySingleAsync<Account>(sql, new { AccountNumber = accountNumber });
        }

        public async Task<bool> AccountNumberExistsAsync(Guid userId, string accountNumber)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty", nameof(accountNumber));

            const string sql = @"
                SELECT COUNT(1) FROM Accounts 
                WHERE UserId = @UserId AND AccountNumber = @AccountNumber
            ";

            var count = await QuerySingleAsync<int>(
                sql,
                new { UserId = userId, AccountNumber = accountNumber }
            );

            return count > 0;
        }
    }
}
