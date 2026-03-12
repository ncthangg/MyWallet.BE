using MyWallet.Application.DTOs.Base.BaseRes;
using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.DTOs.Accounts.Responses
{
    public class GetAccountRes : BaseGetVM
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string? AccountHolder { get; set; }

        public string? BankCode { get; set; }
        public string? NapasCode { get; set; }
        public string? BankName { get; set; }
        public string? ShortName { get; set; }
        public string? LogoUrl { get; set; }

        public AccountProvider Provider { get; set; }

        public decimal? Balance { get; set; }
        public bool IsPinned { get; set; }
        public bool IsActive { get; set; }
    }
}
