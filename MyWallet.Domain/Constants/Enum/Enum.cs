namespace MyWallet.Domain.Constants.Enum
{
    public enum RoleCategory
    {
        Admin,
        User
    }
    public enum PaymentStatus
    {
        PENDING,
        SUCCESS,
        FAILED
    }
    public enum AccountProvider
    {
        BANK,
        MOMO,
        VNPAY,
        ZALOPAY
    }
    public enum QRReceiverType
    {
        PERSONAL,
        GUEST,
    }
    public enum BankCode
    {
        VCB,
        TPB,
        SCB
    }
}
