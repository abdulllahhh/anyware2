using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Seed
    {
        //public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    if (!await roleManager.RoleExistsAsync("Admin"))
        //    {
        //        await roleManager.CreateAsync(new IdentityRole
        //        {
        //            Id = "admin-role-id",
        //            Name = "Admin",
        //            NormalizedName = "ADMIN",
        //            ConcurrencyStamp = "1"
        //        });
        //    }

        //    if (!await roleManager.RoleExistsAsync("CUSTOMER"))
        //    {
        //        await roleManager.CreateAsync(new IdentityRole
        //        {
        //            Id = "customer-role-id",
        //            Name = "Customer",
        //            NormalizedName = "CUSTOMER",
        //            ConcurrencyStamp = "2"
        //        });
        //    }
        //    var existingUser = await userManager.FindByEmailAsync("admin@test.com");

        //    if (existingUser == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "admin@test.com",
        //            Email = "admin@test.com",
        //            EmailConfirmed = true
        //        };

        //        var result = await userManager.CreateAsync(user, "Pa$$w0rd");

        //        if (result.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(user, "Admin");
        //        }
        //    }
        //    await context.SaveChangesAsync();
        //}
    }
}
