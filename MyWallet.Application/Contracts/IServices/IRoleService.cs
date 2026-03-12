using MyWallet.Application.DTOs.Roles.Requests;
using MyWallet.Application.DTOs.Roles.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleRes>> GetAllAsync();
        Task<GetRoleRes> GetByIdAsync(Guid id);
        Task<Guid> PostAsync(PostRoleReq req);
        Task PutAsync(Guid id, PutRoleReq req);
        Task DeleteAsync(Guid id);
    }
}
