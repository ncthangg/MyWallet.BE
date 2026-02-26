using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Contracts.IConfigs
{
    public interface ITokenConfiguration
    {
        string Issuer { get; }
        string Audience { get; }
        string SecretKey { get; }
        int AccessTokenExpirationMinutes { get; }
        int RefreshTokenExpirationDays { get; }
    }
}
