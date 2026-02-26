using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiredTime { get; set; }
        public virtual User? User { get; set; }
    }
}
