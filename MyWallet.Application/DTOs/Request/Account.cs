using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.DTOs.Request
{
    public class PostAccountReq
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string? AccountHolder { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public AccountProvider Provider { get; set; }
        public bool IsActive { get; set; }
    }
    public class PutAccountReq
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string? AccountHolder { get; set; }
        public string? BankCode { get; set; } 
        public string? BankName { get; set; }
        public AccountProvider Provider { get; set; }
        public bool IsPinned { get; set; }
        public bool IsActive { get; set; }
    }
}
