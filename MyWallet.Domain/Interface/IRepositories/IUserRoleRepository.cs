using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Interface.IRepositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<(IEnumerable<UserRole>, int totalCount)> GetAllUserRolesAsync(int pageNumber, int pageSize, Guid? roleId);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
        Task<int> AddUserToRoleAsync(Guid id, Guid userId, Guid roleId);
        Task<int> RemoveUserFromRoleAsync(Guid userId, Guid roleId);
        Task<bool> ExistsAsync(Guid userId, Guid roleId);
    }
}
