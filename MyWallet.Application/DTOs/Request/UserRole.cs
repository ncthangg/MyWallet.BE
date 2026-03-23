namespace MyWallet.Application.DTOs.Request
{
    public class AddUserRoleReq
    {
        public required Guid UserId { get; set; }
        public required Guid RoleId { get; set; }
    }
}
