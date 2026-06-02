using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }
        public async Task<IEnumerable<TaskItem>> GetAllForUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
        public async Task<TaskItem?> GetByTitleAndDateForUserAsync(string title, DateTime date, Guid userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Title == title && t.UserId == userId && t.CreatedAt.Date == date.Date);
        }
        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.Tasks.AddAsync(taskItem);
        }
        public async Task UpdateAsync(TaskItem taskItem)
        {
            _context.Tasks.Update(taskItem);
            await Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
