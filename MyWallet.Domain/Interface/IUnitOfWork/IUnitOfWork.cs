using MyWallet.Domain.Interface.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Interface.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        IQRHistoryRepository QRHistories { get; }
        IUserTokenRepository UserTokens { get; }
        IBankInfoRepository BankInfos { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
