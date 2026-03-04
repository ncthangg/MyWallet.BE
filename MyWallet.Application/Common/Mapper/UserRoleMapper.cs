using MyWallet.Application.DTOs.Response;
using MyWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Common.Mapper
{
    public class UserRoleMapper
    {
        public static GetUserRoleRes ToGetUserRoleRes(UserRole entity)
        {
            return new GetUserRoleRes
            {
                UserId = entity.UserId,
                RoleId = entity.RoleId,
                Name = entity.Role.Name,
                NameUpperCase = entity.Role.NameUpperCase
            };
        }
    }
}
