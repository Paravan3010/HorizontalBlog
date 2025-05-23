using Horizontal.Domain.Contexts;
using Horizontal.Domain.MigrationAndSeed;
using Horizontal.Domain.Repositories;
using Horizontal.Domain.Repositories.EF;
using Horizontal.Middleware;
using Horizontal.Services;
using Horizontal.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Horizontal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<HorizontalDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:MainConnection"]));

            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddRouting();

            builder.Services.AddScoped<ITagRepository, EFTagsRepository>();
            builder.Services.AddScoped<IArticleRepository, EFArticleRepository>();
            builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
            builder.Services.AddScoped<ICustomUrlRepository, EFCustomUrlRepository>();
            builder.Services.AddScoped<IGeneralSettingsRepository, EFGeneralSettingsRepository>();
            builder.Services.AddScoped<ICustomUrlProviderService, CustomUrlProviderService>();
            builder.Services.AddScoped<IArticleTagRepository, EFArticleTagRepository>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            // Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:MainConnection"]));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();

            var app = builder.Build();

            try
            {
                app.UseRequestLocalization(opts =>
                {
                    opts.AddSupportedCultures("cs")
                        .AddSupportedUICultures("cs")
                        .SetDefaultCulture("cs");
                });

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseMiddleware<CustomUrlMiddleware>();

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Main}/{id?}");
                app.MapDefaultControllerRoute();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers(); // This enables attribute routing for controllers.
                });

                app.MapGet("/Admin", context =>
                {
                    context.Response.Redirect("/Admin/General");
                    return Task.CompletedTask;
                });

                MigrationManager.Migrate(app);
                if (app.Environment.IsDevelopment())
                    SeedData.EnsurePopulated(app, builder.Configuration);
                IdentityDataSeed.EnsurePopulated(app, builder.Configuration);

                app.Run();
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw;
            }
        }

        private static void LogException(Exception ex)
        {
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "errors.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);

            var logEntry = $"[{DateTime.Now}] {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry);
        }
    }
}