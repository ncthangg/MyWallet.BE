using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;

namespace MyWallet.Application.Contracts.IRepositories
{
    public interface IQRHistoryRepository
    {
        Task<IEnumerable<QRHistory>> GetByAccountIdAsync(Guid accountId,
                                                         int pageNumber, int pageSize,
                                                         string? sortField, string? sortDirection,
                                                         AccountProvider? provider,
                                                         QRReceiverType? receiverType,
                                                         bool? isFixedAmount, bool? isPaid,
                                                         string? searchValue);
        Task<IEnumerable<QRHistory>> GetByUserIdAsync(Guid userId,
                                                      int pageNumber, int pageSize, 
                                                      DateTime fromDate,
                                                      DateTime toDate);
        Task<decimal> GetTotalQRAmountAsync(Guid accountId,
                                            bool? isPaid,
                                            AccountProvider? provider,
                                            QRReceiverType? receiverType);
    }
}
