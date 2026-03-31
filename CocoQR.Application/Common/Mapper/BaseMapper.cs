using CocoQR.Application.Contracts.ISubServices;

namespace CocoQR.Application.Common.Mapper
{
    public class BaseMapper
    {
        public static string? GetUserName(
        Guid? userId,
        IReadOnlyDictionary<Guid, string>? dict)
        {
            if (userId == null || userId == Guid.Empty) { return null; }
            return dict.TryGetValue(userId.Value, out var name) ? name : null;
        }

        public static string? ResolveFileUrl(string? path, IFileStorageService? fileStorageService)
        {
            if (string.IsNullOrWhiteSpace(path) || fileStorageService == null)
            {
                return path;
            }

            return fileStorageService.GetFileUrl(path);
        }
    }
}
