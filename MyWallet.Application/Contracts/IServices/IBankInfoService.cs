using MyWallet.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IBankInfoService
    {
        Task PostAsync(PostBankInfoReq req);
        Task PutAsync(Guid id, PostBankInfoReq req);
    }
}
