using MyWallet.Domain.Constants.Enum;

namespace MyWallet.Application.DTOs.Accounts.Queries
{
    public class AccountQueryDto
    {
        public Guid Id { get; set; }

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

        public bool? Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Guid? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public Guid? DeletedBy { get; set; }
        public string? DeletedByName { get; set; }
    }
}
