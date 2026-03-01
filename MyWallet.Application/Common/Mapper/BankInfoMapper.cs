using MyWallet.Application.DTOs.Response;
using MyWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Common.Mapper
{
    public class BankInfoMapper
    {

        public static GetBankInfoRes ToGetBankInfoRes(BankInfo u, Dictionary<Guid, string>? userDict)
        {
            return new GetBankInfoRes
            {
                Id = u.Id,
                BankCode = u.BankCode,
                NapasCode = u.NapasCode,
                SwiftCode = u.SwiftCode,
                BankName = u.BankName,

                ShortName = u.ShortName,
                LogoUrl = u.LogoUrl,
                IsActive = u.IsActive,

                Status = u.Status,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                DeletedAt = u.DeletedAt,

                CreatedByName = GetUserName(u.CreatedBy, userDict),
                UpdatedByName = GetUserName(u.UpdatedBy, userDict),
                DeletedByName = GetUserName(u.DeletedBy, userDict),
            };
        }
        private static string? GetUserName(
                Guid? userId,
                IReadOnlyDictionary<Guid, string> dict)
        {
            if (userId == null || userId == Guid.Empty) { return null; }
            return dict.TryGetValue(userId.Value, out var name) ? name : null;
        }


    }
}
