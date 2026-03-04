using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
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
