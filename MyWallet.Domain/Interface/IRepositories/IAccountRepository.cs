using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Interface.IRepositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
        Task<bool> AccountNumberExistsAsync(Guid userId, string accountNumber);
    }
}
