using MyWallet.Application.Contracts.ISubServices;
using UUIDNext;

namespace MyWallet.Infrastructure.SubService
{
    public class SqlServerIdGenerator : IIdGenerator
    {
        public Guid NewId()
            => Uuid.NewDatabaseFriendly(Database.SqlServer);
    }
}
