using Application.DTOs.Task;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;
        private readonly IBackgroundTaskQueue _taskQueue;
        public TaskService(ITaskRepository taskRepository, ICacheService cacheService, IBackgroundTaskQueue taskQueue)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
            _taskQueue = taskQueue;
        }
        public async Task<TaskDto> CreateTaskAsync(CreateTaskRequest request, Guid userId)
        {
            // Business Logic Requirement: Prevent creating duplicate tasks with the same title on the same day for the same user
            var existingTask = await _taskRepository.GetByTitleAndDateForUserAsync(request.Title, DateTime.UtcNow.Date, userId);
            if (existingTask != null)
            {
                throw new AppException("A task with the same title already exists for today.");
            }
            var taskItem = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = Domain.Enums.TaskStatus.Pending,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await _taskRepository.AddAsync(taskItem);
            await _taskRepository.SaveChangesAsync();
            // Send to background processing queue
            await _taskQueue.QueueBackgroundWorkItemAsync(taskItem);
            return MapToDto(taskItem);
        }
        public async Task<TaskDto?> GetTaskByIdAsync(Guid id, Guid userId)
        {
            string cacheKey = $"task_{id}";
            var cachedTask = await _cacheService.GetAsync<TaskDto>(cacheKey);
            
            if (cachedTask != null)
            {
                // Verify ownership of the cached task
                if (cachedTask.UserId != userId)
                {
                    throw new AppException("Forbidden");
                }
                return cachedTask;
            }
            var taskItem = await _taskRepository.GetByIdAsync(id);
            if (taskItem == null || taskItem.UserId != userId)
            {
                return null;
            }
            var taskDto = MapToDto(taskItem);
            await _cacheService.SetAsync(cacheKey, taskDto, TimeSpan.FromMinutes(10));
            return taskDto;
        }
        public async Task<IEnumerable<TaskDto>> GetAllTasksForUserAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetAllForUserAsync(userId);
            
            // Business Logic Requirement: Sort tasks by priority first, then by creation date
            var sortedTasks = tasks
                .OrderByDescending(t => t.Priority)
                .ThenByDescending(t => t.CreatedAt)
                .Select(MapToDto);
            return sortedTasks;
        }
        public async Task<TaskDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusRequest request, Guid userId)
        {
            var taskItem = await _taskRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                throw new NotFoundException("Task not found.");
            }
            if (taskItem.UserId != userId)
            {
                throw new AppException("Forbidden");
            }
            taskItem.Status = request.Status;
            await _taskRepository.UpdateAsync(taskItem);
            await _taskRepository.SaveChangesAsync();
            // Invalidate or refresh the cache
            string cacheKey = $"task_{id}";
            await _cacheService.RemoveAsync(cacheKey);
            return MapToDto(taskItem);
        }
        private static TaskDto MapToDto(TaskItem t)
        {
            return new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId
            };
        }
    }
}