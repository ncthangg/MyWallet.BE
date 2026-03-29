using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CocoQR.Infrastructure.SubService
{
    public interface IFileCleanupQueue
    {
        ValueTask EnqueueAsync(FileCleanupRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<FileCleanupRequest> DequeueAllAsync(CancellationToken cancellationToken);
    }
}
