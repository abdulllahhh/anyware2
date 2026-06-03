using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entities;
using Domain.Enums;
namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly ICurrentUser _currentUser;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _currentUser = currentUser;
        }
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                throw new AppException("Email already in use.");
            }
            var user = User.Create(
                request.Email,
                request.Name,
               UserRole.User,
                request.Password
            );
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            var token = _jwtProvider.GenerateToken(user);
            return new AuthResponse { Token = token, Message = "Registration successful" };
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new AppException("Invalid email or password.");
            }
            var token = _jwtProvider.GenerateToken(user);
            return new AuthResponse { Token = token, Message = "Login successful" };
        }
        public async Task<UserResponse> GetCurrentUser()
        {
            var userId = _currentUser.UserId;

            var user = await _userRepository.GetByIdAsync(userId);
            return new UserResponse
            {
                CreatedAt = user!.CreatedAt,
                Email = user.Email,
                Id = userId,
                Name = user.Name,
                Role = user.Role
            };
        }
    }


}