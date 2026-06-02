using Application.DTOs.User;
namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task DeleteUserAsync(Guid id);
        Task<UserDto?> GetUserByIdAsync(Guid id);
    }
}