using MyWallet.Domain.Interface.IRepositories;

namespace MyWallet.Domain.Interface.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IAccountRepository Accounts { get; }
        IQRHistoryRepository QRHistories { get; }
        IBankInfoRepository BankInfos { get; }

        IUserTokenRepository UserTokens { get; }
        IUserRoleRepository UserRoles { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
