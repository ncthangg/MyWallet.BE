using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IDbContext;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Infrastructure.Persistence.Repositories.Base;

namespace MyWallet.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnectionFactory connectionFactory)
                       : base(connectionFactory, "Roles")
        {
        }
        public async Task<Role?> GetByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be empty", nameof(roleName));

            const string sql = @"
        SELECT 
            Id,
            Name,
            NameUpperCase,
            Description,
            CreatedAt,
            Status
        FROM Roles
        WHERE ( Name = @Name AND NameUpperCase = @NameUpperCase )
    ";

            return await QuerySingleAsync<Role>(sql, new
            {
                Name = roleName.Trim().ToLower(),
                NameUpperCase = roleName.Trim().ToUpper()
            });
        }
    }
}
