using Dapper;
using Microsoft.Identity.Client;
using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Domain.Interface.IUnitOfWork;
using MyWallet.Infrastructure.Persistence.Repositories.Base;
using System.Runtime.CompilerServices;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork _unitOfWork)
            : base(_unitOfWork, "Accounts")
        {
        }

        public async Task<(IEnumerable<Account>, int totalCount)> GetByUserIdAsync(int pageNumber, int pageSize,
                                                                                   Guid? userId,
                                                                                   string? sortField, string? sortDirection,
                                                                                   AccountProvider? provider,
                                                                                   bool? isActive,
                                                                                   string? searchValue)
        {
            var orderBy = "CreatedAt DESC";

            if (!string.IsNullOrEmpty(sortField))
            {
                var dir = sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";

                orderBy = sortField switch
                {
                    "accountNumber" => $"AccountNumber {dir}",
                    "accountHolder" => $"AccountHolder {dir}",
                    "bankCode" => $"BankCode {dir}",
                    "bankName" => $"BankName {dir}",
                    _ => "CreatedAt DESC"
                };
            }

            var orderByFull = userId.HasValue ? $"IsPinned DESC, {orderBy}" : orderBy;

            var sql = $@"
        SELECT
            Id, UserId, AccountNumber, AccountHolder,
            BankCode, BankName, Provider, Balance, IsPinned, IsActive,
            Status, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy
        FROM Accounts
        WHERE
            (@UserId IS NULL OR UserId = @UserId)
            AND (@Provider IS NULL OR Provider = @Provider)
            AND (@IsActive IS NULL OR IsActive = @IsActive)
            AND (
                @SearchValue IS NULL
                OR AccountNumber LIKE '%' + @SearchValue + '%'
                OR ISNULL(AccountHolder,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(BankCode,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(BankName,'') LIKE '%' + @SearchValue + '%'
            )
        ORDER BY 
            {orderByFull}
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Accounts
        WHERE
            (@UserId IS NULL OR UserId = @UserId)
            AND (@Provider IS NULL OR Provider = @Provider)
            AND (@IsActive IS NULL OR IsActive = @IsActive)
            AND (
                @SearchValue IS NULL
                OR AccountNumber LIKE '%' + @SearchValue + '%'
                OR ISNULL(AccountHolder,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(BankCode,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(BankName,'') LIKE '%' + @SearchValue + '%'
            );
        ";

            return await QueryPagedAsync<Account>(sql,
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    UserId = userId,
                    Provider = provider,
                    IsActive = isActive,
                    SearchValue = searchValue
                }
            );
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty", nameof(accountNumber));

            const string sql = "SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber";

            return await QueryFirstOrDefaultAsync<Account>(sql,
                new
                {
                    AccountNumber = accountNumber
                }
            );
        }

        public async Task<bool> AccountNumberExistsAsync(Guid userId, string accountNumber, string? bankCode, AccountProvider provider, Guid? excludeAccountId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty", nameof(accountNumber));

            const string sql = @"
                SELECT TOP 1 1
                FROM Accounts
                WHERE UserId = @UserId
                      AND AccountNumber = @AccountNumber
                      AND (@BankCode IS NULL OR BankCode = @BankCode)
                      AND Provider = @Provider
                      AND (@ExcludeId IS NULL OR Id <> @ExcludeId)
                ";

            var count = await QueryFirstOrDefaultAsync<int>(sql,
                new
                {
                    UserId = userId,
                    AccountNumber = accountNumber,
                    BankCode = bankCode,
                    Provider = provider,
                    ExcludeId = excludeAccountId
                }
            );

            return count > 0;
        }
    }
}
