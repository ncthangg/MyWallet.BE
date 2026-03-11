using MyWallet.Application.DTOs.Response.Base;
using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.DTOs.Response
{
    public class GetAccountRes : BaseGetVM
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public AccountProvider Provider { get; set; }

        public decimal? Balance { get; set; }
        public bool IsPinned { get; set; }
        public bool IsActive { get; set; }
    }
}
