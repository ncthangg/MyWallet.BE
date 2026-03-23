using MyWallet.Application.DTOs.QRStyleLibrary.Requests;
using MyWallet.Application.DTOs.QRStyleLibrary.Responses;
using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.Contracts.IServices
{
    public interface IQrStyleLibraryService
    {
        Task<IEnumerable<GetQrStyleLibraryRes>> GetAllAsync(QRStyleType? type, bool? isActive);

        Task<Guid> PostUserStyleAsync(PostQRStyleReq request);
        Task PutUserStyleAsync(Guid styleId, PutQRStyleReq request);
        Task DeleteUserStyleAsync(Guid styleId);
    }
}
