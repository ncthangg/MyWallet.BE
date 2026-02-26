using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;

namespace MyWallet.Application.Contracts.ISubServices
{
    public interface IGoogleService
    {
        string BuildSuccessHtml(BaseResponseModel<SignInGoogleRes> response, string origin);
    }
}
