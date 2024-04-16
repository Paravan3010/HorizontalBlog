using Horizontal.Domain.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.MigrationAndSeed
{
    public class IdentityDataSeed
    {
        public static async void EnsurePopulated(IApplicationBuilder app, ConfigurationManager config)
        {
            var adminUser = config["AdminUser:Login"];
            var adminPassword = config["AdminUser:Password"];

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByNameAsync(adminUser);
            if (user != null)
                return;
            // populate
            user = new IdentityUser(adminUser);
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}
