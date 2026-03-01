using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Interface.IRepositories
{
    public interface IBankInfoRepository : IRepository<BankInfo>
    {
        Task<(IEnumerable<BankInfo>, int totalCount)> GetBankInfosAsync(int pageNumber, int pageSize, bool? isActive, string? searchValue);

    }
}
