using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Presentation.DependencyInjection;
using Presentation.Middleware;

namespace Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        builder.Services.AddPresentation(builder.Configuration);

        builder.Services.AddInfrastructure(builder.Configuration);


        builder.Services.AddAuthorization();



        var app = builder.Build();

        // Middleware
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Database Seed
        await DatabaseSeeder.SeedAsync(app.Services);

        await app.RunAsync();
    }
}