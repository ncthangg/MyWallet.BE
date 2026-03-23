using MyWallet.Application.DTOs.Base.BaseRes;

namespace MyWallet.Application.DTOs.Response
{
    public class GetRoleRes : BaseGetVM<Guid>
    {
        public required string Name { get; set; }
        public required string NameUpperCase { get; set; }
    }
}
