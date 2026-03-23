using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.DTOs.QRStyleLibrary.Requests
{
    public class GetQrStyleLibraryReq
    {
        public Guid? UserId { get; set; }
        public QRStyleType? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
