using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IAccountService
    {
        Task<GetAccountRes> GetByIdAsync(Guid accountId);
        Task<PagingVM<GetAccountRes>> GetUserAccountsAsync(Guid userId, int pageNumber = 1, int pageSize = 10, bool? isActive = true);
        Task<Guid> PostAccountAsync(PostAccountReq request);
        Task PutAccountAsync(Guid accountId, PutAccountReq request);
        Task DeleteAccountAsync(Guid accountId);
    }
}
