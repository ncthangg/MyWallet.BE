using MyWallet.Application.DTOs.Roles.Responses;
using MyWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Common.Mapper
{
    public class RoleMapper
    {
        public static GetRoleRes ToGetRoleRes(Role u)
        {
            return new GetRoleRes
            {
                Id = u.Id,
                Name = u.Name,
                NameUpperCase = u.NameUpperCase,

                Status = u.Status,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                DeletedAt = u.DeletedAt,

            };
        }
    }
}
