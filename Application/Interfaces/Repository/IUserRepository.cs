using Application.DTOs.RequestDtos;
using Domain.Entities;
namespace Application.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<PagedResult<User>> GetPagedAsync(PaginationRequest request);
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(User user);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}