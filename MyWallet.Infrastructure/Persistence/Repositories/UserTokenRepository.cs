using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IDbContext;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(IDbConnectionFactory connectionFactory)
                       : base(connectionFactory, "UserToken")
        {
        }
    }
}
