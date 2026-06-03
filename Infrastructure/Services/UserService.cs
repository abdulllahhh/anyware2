using Application.DTOs.RequestDtos;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });
        }
        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }// handle null response
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new AppException("User not found.");
            }
            // Cannot delete admin user
            if (user.Role == Domain.Enums.UserRole.Admin)
            {
                throw new AppException("Cannot delete the admin user.");
            }
            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            // Validate
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                throw new AppException($"Email {request.Email} is already taken");

            // Create user
            var user = User.Create(
                request.Email,
                request.Name,
                request.Role,
                request.Password
            );

            // Save
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToDto(user);
        }
        public async Task<PagedResult<UserDto>> GetUsersAsync(PaginationRequest request)
        {
            var result = await _userRepository.GetPagedAsync(request);

            return new PagedResult<UserDto>
            {
                Items = result.Items.Select(MapToDto).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task SoftDeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");
            if (user.Role == Domain.Enums.UserRole.Admin)
            {
                throw new AppException("Cannot delete the admin user.");
            }
            user.SoftDelete();

            await _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}