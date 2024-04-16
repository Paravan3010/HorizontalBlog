using Horizontal.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Horizontal.Domain.MigrationAndSeed
{
    public static class MigrationManager
    {
        public static void Migrate(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HorizontalDbContext>();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
            var identityContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            if (identityContext.Database.GetPendingMigrations().Any())
                identityContext.Database.Migrate();
        }
    }
}
