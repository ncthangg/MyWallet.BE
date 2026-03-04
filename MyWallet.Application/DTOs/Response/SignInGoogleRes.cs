namespace MyWallet.Application.DTOs.Response
{
    public class SignInGoogleRes
    {
        public required GetUserRes UserRes { get; set; }
        public required TokenRes TokenRes { get; set; }
        public required IEnumerable<GetRoleRes> RoleRes { get; set; }
    }
    public class TokenRes
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
