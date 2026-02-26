using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;
using IDbConnectionFactory = MyWallet.Domain.Interface.IDbContext.IDbConnectionFactory;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class QRHistoryRepository : BaseRepository<QRHistory>, IQRHistoryRepository
    {
        public QRHistoryRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory, "QRHistories")
        {
        }

        public async Task<IEnumerable<QRHistory>> GetByAccountIdAsync(Guid accountId, int pageSize = 20)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            const string sql = @"
                SELECT TOP (@PageSize)
                    Id, UserId, AccountId, Amount, Description, QRData, CreatedAt
                FROM QRHistories
                WHERE AccountId = @AccountId
                ORDER BY CreatedAt DESC
            ";

            return await QueryAsync<QRHistory>(
                sql,
                new { AccountId = accountId, PageSize = pageSize }
            );
        }

        public async Task<IEnumerable<QRHistory>> GetByUserIdAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));
            if (fromDate > toDate)
                throw new ArgumentException("FromDate must be before ToDate");

            const string sql = @"
                SELECT 
                    Id, UserId, AccountId, Amount, Description, QRData, CreatedAt
                FROM QRHistories
                WHERE UserId = @UserId 
                    AND CreatedAt BETWEEN @FromDate AND @ToDate
                ORDER BY CreatedAt DESC
            ";

            return await QueryAsync<QRHistory>(
                sql,
                new
                {
                    UserId = userId,
                    FromDate = fromDate,
                    ToDate = toDate
                }
            );
        }

        public async Task<decimal> GetTotalQRAmountAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            const string sql = @"
                SELECT COALESCE(SUM(Amount), 0)
                FROM QRHistories
                WHERE AccountId = @AccountId
            ";

            return await QuerySingleAsync<decimal>(
                sql,
                new { AccountId = accountId }
            );
        }
    }
}
