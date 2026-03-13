using Dapper;
using Microsoft.Identity.Client;
using MyWallet.Application.Contracts.IRepositories;
using MyWallet.Application.Contracts.IUnitOfWork;
using MyWallet.Application.DTOs.Accounts.Queries;
using MyWallet.Application.DTOs.Accounts.Responses;
using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;
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

        public async Task<(IEnumerable<AccountQueryDto>, int totalCount)> GetAllAsync(int pageNumber, int pageSize,
                                                                                   string? sortField, string? sortDirection,
                                                                                   Guid? userId,
                                                                                   AccountProvider? provider,
                                                                                   string? searchValue,
                                                                                   bool? isActive,
                                                                                   bool? isDeleted,
                                                                                   bool? status)
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
            a.Id, a.UserId, a.AccountNumber, a.AccountHolder,
            a.BankCode, b.BankName AS BankName, b.LogoUrl,
            a.Provider, a.Balance, a.IsPinned, a.IsActive,

            a.Status,

            a.CreatedBy,

            u1.FullName AS CreatedByName,

            a.CreatedAt
        FROM Accounts a
            LEFT JOIN BankInfos b
                 ON a.BankCode = b.BankCode
            LEFT JOIN Users u ON a.UserId = u.Id
            LEFT JOIN Users u1 ON a.CreatedBy = u1.Id
            LEFT JOIN Users u2 ON a.UpdatedBy = u2.Id
            LEFT JOIN Users u3 ON a.DeletedBy = u3.Id        
        WHERE
            (@UserId IS NULL OR a.UserId = @UserId)
            AND (@Provider IS NULL OR a.Provider = @Provider)
            AND (@IsActive IS NULL OR a.IsActive = @IsActive)
            AND (
                 @IsDeleted IS NULL
                 OR (@IsDeleted = 1 AND a.DeletedAt IS NOT NULL)
                 OR (@IsDeleted = 0 AND a.DeletedAt IS NULL)
            )
            AND (@Status IS NULL OR a.Status = @Status)
            AND (
                @SearchValue IS NULL
                OR a.AccountNumber LIKE '%' + @SearchValue + '%'
                OR ISNULL(a.AccountHolder,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(a.BankCode,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(b.BankName,'') LIKE '%' + @SearchValue + '%'
                OR u.Email = @SearchValue
            )
        ORDER BY 
            {orderByFull}
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Accounts a
            LEFT JOIN BankInfos b
                 ON a.BankCode = b.BankCode
            LEFT JOIN Users u ON a.UserId = u.Id
        WHERE
            (@UserId IS NULL OR a.UserId = @UserId)
            AND (@Provider IS NULL OR a.Provider = @Provider)
            AND (@IsActive IS NULL OR a.IsActive = @IsActive)
            AND (
                 @IsDeleted IS NULL
                 OR (@IsDeleted = 1 AND a.DeletedAt IS NOT NULL)
                 OR (@IsDeleted = 0 AND a.DeletedAt IS NULL)
            )
            AND (@Status IS NULL OR a.Status = @Status)
            AND (
                @SearchValue IS NULL
                OR a.AccountNumber LIKE '%' + @SearchValue + '%'
                OR ISNULL(a.AccountHolder,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(a.BankCode,'') LIKE '%' + @SearchValue + '%'
                OR ISNULL(b.BankName,'') LIKE '%' + @SearchValue + '%'
                OR u.Email = @SearchValue
            );
        ";

            return await QueryPagedAsync<AccountQueryDto>(sql,
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    UserId = userId,
                    Provider = provider,
                    SearchValue = searchValue,
                    IsActive = isActive,
                    IsDeleted = isDeleted,
                    Status = status
                }
            );
        }
        public async Task<AccountQueryDto?> GetByIdAsync(Guid id, Guid? userId, bool isAdmin)
        {
            string sql;

            if (isAdmin)
            {
                sql = $@"SELECT
                     a.Id, a.UserId, a.AccountNumber, a.AccountHolder,
                     a.BankCode, b.BankName AS BankName, b.LogoUrl,
                     a.Provider, a.Balance, a.IsPinned, a.IsActive,

                     a.Status,

                     a.CreatedBy,

                     u1.FullName AS CreatedByName,

                     a.CreatedAt,
                     a.UpdatedAt
                FROM Accounts
                LEFT JOIN BankInfos b
                    ON a.BankCode = b.BankCode
                WHERE a.Id = @Id";
            }
            else
            {
                sql = $@"SELECT
                     a.Id, a.UserId, a.AccountNumber, a.AccountHolder,
                     a.BankCode, b.BankName AS BankName, b.LogoUrl,
                     a.Provider, a.Balance, a.IsPinned, a.IsActive,

                     a.Status,

                     a.CreatedBy,
                     a.UpdatedBy,
                     a.DeletedBy,

                     u1.FullName AS CreatedByName,
                     u2.FullName AS UpdatedByName,
                     u3.FullName AS DeletedByName,

                     a.CreatedAt,
                     a.UpdatedAt,
                     a.DeletedAt
                FROM Accounts a
                LEFT JOIN BankInfos b
                    ON a.BankCode = b.BankCode
                WHERE a.Id = @Id
                     AND a.UserId = @UserId
                     AND a.DeletedAt IS NULL
                     AND a.Status = 1
                ";
            }

            return await QueryFirstOrDefaultAsync<AccountQueryDto>(sql,
                new
                {
                    Id = id,
                    UserId = userId
                });
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
                     AND DeletedAt IS NULL
                     AND Status = 1
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
