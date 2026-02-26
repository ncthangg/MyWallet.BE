using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.DTOs.Request
{
    public class PostAccountReq
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal? Balance { get; set; }
    }
    public class PutAccountReq
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal? Balance { get; set; }
    }
}
