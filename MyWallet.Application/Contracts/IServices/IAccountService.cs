using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;
using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IAccountService
    {
        Task<PagingVM<GetAccountRes>> GetUserAccountsAsync(int pageNumber, int pageSize,
                                                           Guid? userId,
                                                           string? sortField, string? sortDirection,
                                                           AccountProvider? provider,
                                                           bool? isActive, 
                                                           string? searchValue);
        Task<GetAccountRes> GetByIdAsync(Guid id);
        Task<Guid> PostAccountAsync(PostAccountReq request);
        Task PutAccountAsync(Guid id, PutAccountReq request);
        Task DeleteAccountAsync(Guid id);
    }
}
