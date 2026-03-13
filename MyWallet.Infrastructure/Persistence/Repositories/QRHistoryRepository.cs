using Dapper;
using MyWallet.Application.Contracts.IRepositories;
using MyWallet.Application.Contracts.IUnitOfWork;
using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;
using MyWallet.Infrastructure.Persistence.Repositories.Base;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class QRHistoryRepository : BaseRepository<QRHistory>, IQRHistoryRepository
    {
        public QRHistoryRepository(IUnitOfWork _unitOfWork)
            : base(_unitOfWork, "QRHistories")
        {
        }

        public async Task<IEnumerable<QRHistory>> GetByAccountIdAsync(Guid accountId,
            int pageNumber, int pageSize,
            string? sortField, string? sortDirection,
            AccountProvider? provider,
            QRReceiverType? receiverType,
            bool? isFixedAmount, bool? isPaid,
            string? searchValue)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            var orderBy = "CreatedAt DESC";

            if (!string.IsNullOrEmpty(sortField))
            {
                var dir = sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";

                orderBy = sortField switch
                {
                    "accountNumberSnapshot" => $"AccountNumberSnapshot {dir}",
                    "accountHolderSnapshot" => $"AccountHolderSnapshot {dir}",
                    "bankCodeSnapshot" => $"BankCodeSnapshot {dir}",
                    "bankNameSnapshot" => $"BankNameSnapshot {dir}",
                    "description" => $"Description {dir}",
                    _ => "CreatedAt DESC"
                };
            }

            var sql = $@"
                SELECT
                    Id, UserId, AccountId,
                    AccountNumberSnapshot, AccountHolderSnapshot, BankCodeSnapshot, BankNameSnapshot
                    Amount, Description, Provider, ReceiverType,
                    IsFixedAmount, IsPaid,
                    CreatedAt, ExpiredAt, PaidAt, DeletedAt
                FROM QRHistories
                WHERE
                    (AccountId = @AccountId)
                    AND (@Provider IS NULL OR Provider = @Provider)
                    AND (@ReceiverType IS NULL OR ReceiverType = @ReceiverType)
                    AND (@IsFixedAmount IS NULL OR IsFixedAmount = @IsFixedAmount)
                    AND (@IsPaid IS NULL OR IsPaid = @IsPaid)
                    AND (
                        @SearchValue IS NULL
                        OR ISNULL(AccountNumberSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(AccountHolderSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(BankCodeSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(BankNameSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(Description,'') LIKE '%' + @SearchValue + '%'
                    )
                ORDER BY {orderBy}
                OFFSET (@PageNumber - 1) * @PageSize ROWS
                FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1)
                FROM QRHistories
                WHERE
                    (AccountId = @AccountId)
                    AND (@Provider IS NULL OR Provider = @Provider)
                    AND (@ReceiverType IS NULL OR ReceiverType = @ReceiverType)
                    AND (@IsFixedAmount IS NULL OR IsFixedAmount = @IsFixedAmount)
                    AND (@IsPaid IS NULL OR IsPaid = @IsPaid)
                    AND (
                        @SearchValue IS NULL
                        OR ISNULL(AccountNumberSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(AccountHolderSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(BankCodeSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(BankNameSnapshot,'') LIKE '%' + @SearchValue + '%'
                        OR ISNULL(Description,'') LIKE '%' + @SearchValue + '%'
                    )
                ";

            return await QueryAsync<QRHistory>(sql,
                new
                {
                    AccountId = accountId,
                    Provider = provider?.ToString(),
                    ReceiverType = receiverType?.ToString(),
                    IsFixedAmount = isFixedAmount,
                    IsPaid = isPaid,
                    SearchValue = searchValue,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            );
        }

        public async Task<IEnumerable<QRHistory>> GetByUserIdAsync(
            Guid userId,
            int pageNumber, int pageSize,
            DateTime fromDate,
            DateTime toDate)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));
            if (fromDate > toDate)
                throw new ArgumentException("FromDate must be before ToDate");

            const string sql = @"
                SELECT 
                    Id, UserId, AccountId,
                    AccountNumberSnapshot, AccountHolderSnapshot, BankCodeSnapshot, BankNameSnapshot
                    Amount, Description, QRData, QRImageUrl, Provider, ReceiverType,
                    IsFixedAmount, IsPaid,
                    CreatedAt, ExpiredAt, PaidAt, DeletedAt
                FROM QRHistories
                WHERE 
                      UserId = @UserId 
                      AND CreatedAt >= @FromDate
                      AND CreatedAt < DATEADD(DAY,1,@ToDate)
                ORDER BY CreatedAt DESC
                OFFSET (@PageNumber - 1) * @PageSize ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            ";

            return await QueryAsync<QRHistory>(sql,
                new
                {
                    UserId = userId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            );
        }

        public async Task<decimal> GetTotalQRAmountAsync(Guid accountId,
                                                         bool? isPaid,
                                                         AccountProvider? provider,
                                                         QRReceiverType? receiverType)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            const string sql = @"
                SELECT COALESCE(SUM(Amount), 0)
                FROM QRHistories
                WHERE 
                     AccountId = @AccountId
                     AND (@IsPaid IS NULL OR IsPaid = @IsPaid)
                     AND (@Provider IS NULL OR Provider = @Provider)
                     AND (@ReceiverType IS NULL OR ReceiverType = @ReceiverType)
            ";

            return await QueryFirstOrDefaultAsync<decimal>(sql,
                new
                {
                    AccountId = accountId,
                    IsPaid = isPaid,
                    Provider = provider,
                    ReceiverType = receiverType
                }
            );
        }
    }
}
