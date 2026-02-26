using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IAccountService
    {
        Task<GetAccountRes> GetAccountAsync(Guid accountId);
        Task<IEnumerable<GetAccountRes>> GetUserAccountsAsync(Guid userId);
        Task<Guid> CreateAccountAsync(PostAccountReq request);
        Task UpdateAccountAsync(Guid accountId, PutAccountReq request);
        Task DeleteAccountAsync(Guid accountId);
    }
}
