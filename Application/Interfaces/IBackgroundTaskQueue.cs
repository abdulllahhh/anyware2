using Domain.Entities;
namespace Application.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(TaskItem taskItem);
        ValueTask<TaskItem> DequeueAsync(CancellationToken cancellationToken);
    }
}