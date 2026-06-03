using Domain.Entities;
namespace Application.Interfaces.Repository
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetAllForUserAsync(Guid userId);
        Task<TaskItem?> GetByTitleAndDateForUserAsync(string title, DateTime date, Guid userId);
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task SaveChangesAsync();
    }
}