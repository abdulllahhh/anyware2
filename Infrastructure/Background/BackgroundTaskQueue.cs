using System.Threading.Channels;
using Application.Interfaces;
using Domain.Entities;
namespace Infrastructure.Background
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<TaskItem> _queue;
        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<TaskItem>(options);
        }
        public async ValueTask QueueBackgroundWorkItemAsync(TaskItem taskItem)
        {
            await _queue.Writer.WriteAsync(taskItem);
        }
        public async ValueTask<TaskItem> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}