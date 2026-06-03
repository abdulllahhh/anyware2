using Application.DTOs.RequestDtos;
using Application.DTOs.User;
namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<PagedResult<UserDto>> GetUsersAsync(PaginationRequest request);
        Task DeleteUserAsync(Guid id);
        Task SoftDeleteUserAsync(Guid id);
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(CreateUserRequest request);

    }
}