using MyWallet.Application.Contracts.IRepositories.Base;
using MyWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IRepositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
        Task<int> AddUserToRoleAsync(Guid id, Guid userId, Guid roleId);
        Task<int> RemoveUserFromRoleAsync(Guid userId, IEnumerable<Guid> roleIds);
        Task<bool> ExistsAsync(Guid userId, Guid roleId);
    }
}
