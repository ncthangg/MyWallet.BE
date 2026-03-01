using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IDbContext;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;
namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class BankInfoRepository : BaseRepository<BankInfo>, IBankInfoRepository
    {
        public BankInfoRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory, "BankInfos")
        {
        }
        public async Task<(IEnumerable<BankInfo>, int totalCount)> GetBankInfosAsync(int pageNumber, int pageSize, bool? isActive, string? searchValue)
        {

            const string sql = @"
        SELECT 
            Id, BankCode, NapasCode, SwiftCode, 
            BankName, ShortName, LogoUrl, IsActive,
            Status, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy
        FROM BankInfos
        WHERE 
            (@IsActive IS NULL OR IsActive = @IsActive)
            AND (
                @SearchValue IS NULL 
                OR BankName LIKE '%' + @SearchValue + '%'
                OR ShortName LIKE '%' + @SearchValue + '%'
                OR BankCode LIKE '%' + @SearchValue + '%'
            )
        ORDER BY BankName ASC
        OFFSET (@PageNumber - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM BankInfos
        WHERE 
            (@IsActive IS NULL OR IsActive = @IsActive)
            AND (
                @SearchValue IS NULL 
                OR BankName LIKE '%' + @SearchValue + '%'
                OR ShortName LIKE '%' + @SearchValue + '%'
                OR BankCode LIKE '%' + @SearchValue + '%'
            );
    ";

            return await QueryPagedAsync<BankInfo>(sql, new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                IsActive = isActive,
                SearchValue = searchValue
            });
        }
    }
}
