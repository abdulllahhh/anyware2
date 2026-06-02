using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
            try
            {
                if (context.Database.IsRelational())
                {
                    await context.Database.MigrateAsync();
                }
                if (!await context.Users.AnyAsync(u => u.Email == "admin@example.com"))
                {
                    logger.LogInformation("Seeding default admin user.");
                    var adminUser = new User
                    {
                        Name = "Admin User",
                        Email = "admin@example.com",
                        PasswordHash = passwordHasher.HashPassword("Admin@123"),
                        Role = UserRole.Admin,
                        CreatedAt = DateTime.UtcNow
                    };
                    await context.Users.AddAsync(adminUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}