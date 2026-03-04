using MyWallet.Application.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.DTOs.Response
{
    public class GetUserRoleRes : BaseGetVM
    {
        public required Guid UserId { get; set; }
        public required Guid RoleId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string NameUpperCase { get; set; } = string.Empty;
    }
}
