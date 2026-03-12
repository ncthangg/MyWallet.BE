using MyWallet.Application.Contracts.IRepositories;
using MyWallet.Domain.Entities;

namespace MyWallet.Application.Common.Helper
{
    public static class UserHelper
    {
        public static async Task<Dictionary<Guid, string>> GetUserNameDictAsync(BaseEntity item, IUserRepository userRepository)
        {
            var userIds = new Guid?[] { item.CreatedBy, item.UpdatedBy, item.DeletedBy }
            .Where(id => id.HasValue && id.Value != Guid.Empty).Select(id => id!.Value).Distinct().ToList();

            if (userIds.Count == 0)
                return [];

            var users = await userRepository.GetUsersByIdsAsync(userIds);
            return (users ?? Enumerable.Empty<User>()).ToDictionary(x => x.Id, x => x.FullName);
        }
        public static async Task<Dictionary<Guid, string>> GetUserNameDictAsync<T>(List<T> items, IUserRepository userRepository) where T : BaseEntity
        {
            var userIds = items.SelectMany(p => new Guid?[] { p.CreatedBy, p.UpdatedBy, p.DeletedBy })
                .Where(id => id.HasValue && id.Value != Guid.Empty).Select(id => id!.Value).Distinct().ToList();

            if (userIds.Count == 0)
                return [];

            var users = await userRepository.GetUsersByIdsAsync(userIds);
            return (users ?? Enumerable.Empty<User>()).ToDictionary(x => x.Id, x => x.FullName);
        }
    }
}
