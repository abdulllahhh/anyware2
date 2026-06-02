using Application.DTOs.Task;
namespace Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskRequest request, Guid userId);
        Task<TaskDto?> GetTaskByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<TaskDto>> GetAllTasksForUserAsync(Guid userId);
        Task<TaskDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusRequest request, Guid userId);
    }
}