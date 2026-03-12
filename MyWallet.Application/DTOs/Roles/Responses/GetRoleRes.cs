using MyWallet.Application.DTOs.Base.BaseRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.DTOs.Roles.Responses
{
    public class GetRoleRes : BaseGetVM
    {
        public required string Name { get; set; }
        public required string NameUpperCase { get; set; }
    }
}
