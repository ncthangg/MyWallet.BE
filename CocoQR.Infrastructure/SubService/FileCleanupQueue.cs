using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CocoQR.Infrastructure.SubService
{
    public sealed class FileCleanupQueue : IFileCleanupQueue
    {
        private readonly Channel<FileCleanupRequest> _channel = Channel.CreateUnbounded<FileCleanupRequest>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

        public ValueTask EnqueueAsync(FileCleanupRequest request, CancellationToken cancellationToken = default)
        {
            return _channel.Writer.WriteAsync(request, cancellationToken);
        }

        public IAsyncEnumerable<FileCleanupRequest> DequeueAllAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
