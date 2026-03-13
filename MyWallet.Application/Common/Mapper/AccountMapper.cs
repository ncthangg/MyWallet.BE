using MyWallet.Application.DTOs.Accounts.Queries;
using MyWallet.Application.DTOs.Accounts.Responses;

namespace MyWallet.Application.Common.Mapper
{
    public class AccountMapper
    {
        public static GetAccountRes ToGetAccountRes(AccountQueryDto u)
        {
            return new GetAccountRes
            {
                Id = u.Id,
                UserId = u.UserId,
                AccountNumber = u.AccountNumber,
                AccountHolder = u.AccountHolder ?? "",

                BankCode = u.BankCode ?? "",
                NapasCode = u.NapasCode ??"",
                BankName = u.BankName ?? "",
                ShortName = u.ShortName ?? "",
                LogoUrl = u.LogoUrl ?? "",

                Provider = u.Provider,
                Balance = u.Balance,

                IsPinned = u.IsPinned,
                IsActive = u.IsActive,

                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,

                CreatedBy = u.CreatedBy,
                UpdatedBy = u.UpdatedBy,

                CreatedByName = u.CreatedByName,
                UpdatedByName = u.UpdatedByName,
            };
        }
        public static GetAccountRes ToGetAccountByAdminRes(AccountQueryDto u)
        {
            return new GetAccountRes
            {
                Id = u.Id,
                UserId = u.UserId,
                AccountNumber = u.AccountNumber,
                AccountHolder = u.AccountHolder ?? "",

                BankCode = u.BankCode ?? "",
                NapasCode = u.NapasCode ?? "",
                BankName = u.BankName ?? "",
                ShortName = u.ShortName ?? "",
                LogoUrl = u.LogoUrl ?? "",

                Provider = u.Provider,
                Balance = u.Balance,

                IsPinned = u.IsPinned,
                IsActive = u.IsActive,

                Status = u.Status,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                DeletedAt = u.DeletedAt,

                CreatedBy = u.CreatedBy,
                UpdatedBy = u.UpdatedBy,
                DeletedBy = u.DeletedBy,

                CreatedByName = u.CreatedByName,
                UpdatedByName = u.UpdatedByName,
                DeletedByName = u.DeletedByName,
            };
        }
    }
}
