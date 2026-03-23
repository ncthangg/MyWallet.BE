using MyWallet.Domain.Constants.Enum;
using MyWallet.Domain.Entities;

namespace MyWallet.Application.Contracts.IRepositories
{
    public interface IQRStyleLibraryRepository
    {
        Task<IEnumerable<QRStyleLibrary>> GetAllAsync(Guid? userId, QRStyleType? type, bool? isActive, bool isAdmin);

        Task<QRStyleLibrary?> GetByIdAsync(Guid id);
        Task AddAsync(QRStyleLibrary entity);
        Task UpdateAsync(QRStyleLibrary entity);
        Task DeleteAsync(Guid id);
    }
}
