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
    }
}
