using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Interface.IRepositories
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
    }
}
