using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Background
{
    public class TaskProcessingBackgroundService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<TaskProcessingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public TaskProcessingBackgroundService(
            IBackgroundTaskQueue taskQueue,
            ILogger<TaskProcessingBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Processing Background Service is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var taskItem = await _taskQueue.DequeueAsync(stoppingToken);
                try
                {
                    // Create scope to resolve scoped services
                    using var scope = _serviceProvider.CreateScope();
                    var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                    _logger.LogInformation($"Processing Task Id: {taskItem.Id}, Title: {taskItem.Title}");
                    // Simulate processing time
                    await Task.Delay(3000, stoppingToken);
                    // Update task status to Done
                    var dbTask = await taskRepository.GetByIdAsync(taskItem.Id);
                    if (dbTask != null)
                    {
                        dbTask.Status = Domain.Enums.TaskStatus.Done;
                        await taskRepository.UpdateAsync(dbTask);
                        await taskRepository.SaveChangesAsync();
                        _logger.LogInformation($"Task Id: {taskItem.Id} successfully processed and marked as Done.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred executing task processing for Task Id: {taskItem.Id}.");
                }
            }
        }
    }
}