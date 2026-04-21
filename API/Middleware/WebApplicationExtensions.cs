using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware;

public static class WebApplicationExtensions
{
    public static async Task InitializeDB(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        try
        {
            // Auto-migrate the database
            await dbContext.Database.MigrateAsync();
        }
        catch (SqliteException ex) when (app.Environment.IsDevelopment() && ex.SqliteErrorCode == 26)
        {
            // "file is not a database" - recreate local dev db files and retry once.
            dbContext.Database.CloseConnection();
            SqliteConnection.ClearAllPools();

            var dbPath = Path.Combine(app.Environment.ContentRootPath, "app.db");
            foreach (var path in new[] { $"{dbPath}-wal", $"{dbPath}-shm", dbPath })
            {
                for (var attempt = 0; attempt < 5; attempt++)
                {
                    if (!File.Exists(path))
                    {
                        break;
                    }

                    try
                    {
                        File.Delete(path);
                        break;
                    }
                    catch (IOException) when (attempt < 4)
                    {
                        await Task.Delay(150);
                        SqliteConnection.ClearAllPools();
                    }
                }
            }

            await dbContext.Database.MigrateAsync();
        }


        // Seed initial data (roles, users, etc.)
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        await DataSeeder.SeedAsync(dbContext, userManager, roleManager);
    }
}
