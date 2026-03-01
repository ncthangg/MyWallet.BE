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
    public interface IBankInfoService
    {

        Task<PagingVM<GetBankInfoRes>> GetsAsync(int pageNumber, int pageSize, bool? isActive, string? searchValue);
        Task PostAsync(PostBankInfoReq req);
        Task PutAsync(Guid id, PutBankInfoReq req);
    }
}
