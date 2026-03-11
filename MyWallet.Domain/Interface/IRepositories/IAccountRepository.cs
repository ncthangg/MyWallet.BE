using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories.Base;

namespace MyWallet.Domain.Interface.IRepositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<(IEnumerable<Account>, int totalCount)> GetByUserIdAsync(int pageNumber, int pageSize,
            Guid? userId,
            string? sortField, string? sortDirection,
            AccountProvider? provider,
            bool? isActive, 
            string? searchValue);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
        Task<bool> AccountNumberExistsAsync(Guid userId, string accountNumber, string? bankCode, AccountProvider provider, Guid? excludeAccountId = null);
    }
}
