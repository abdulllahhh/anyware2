using Domain.Enums;
namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }


        private User() { }
        public void SoftDelete()
        {
            if (Role == UserRole.Admin)
                throw new InvalidOperationException("Admin users cannot be deleted.");

            IsDeleted = true;
        }
        public static User Create(
        string email,
        string Name,
        UserRole role,
        string password)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = email.ToLower(),
                Name = Name.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                CreatedAt = DateTime.UtcNow,
            };
        }

    }
}