using MyWallet.Application.DTOs.Base.BaseRes;

namespace MyWallet.Application.DTOs.Response
{
    public class GetUserRoleRes : BaseGetVM<Guid>
    {
        public required Guid UserId { get; set; }
        public required Guid RoleId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string NameUpperCase { get; set; } = string.Empty;
    }
}
