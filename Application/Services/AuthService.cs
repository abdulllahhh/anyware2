using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.GetByEmailAsync(request.Email) != null)
            {
                throw new AppException("Email already in use.");
            }
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = Domain.Enums.UserRole.User
            };
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
    }
}