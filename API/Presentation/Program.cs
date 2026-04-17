using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
    });
});

builder.Services.AddDataProtection();
builder.Services.AddOpenApi();

// Embed config only in production
if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["ConnectionStrings:DefaultConnection"] = "Data Source=app.db",
        ["JwtSettings:SecretKey"] = "your-super-secret-key-at-least-32-chars",
        ["JwtSettings:Issuer"] = "your-app",
        ["JwtSettings:Audience"] = "your-app",
        ["JwtSettings:ExpiryMinutes"] = "60",
        ["AllowedOrigins:0"] = "http://localhost:5000"
    });
}

var app = builder.Build();

await app.InitializeDB();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // Production - serve Angular from embedded resources
    var embeddedProvider = new ManifestEmbeddedFileProvider(
        typeof(Program).Assembly, "wwwroot");

    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = embeddedProvider
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = embeddedProvider
    });
    app.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = embeddedProvider
    });

    // Auto open browser only in production
    Task.Run(async () =>
    {
        await Task.Delay(1500);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "http://localhost:5000",
            UseShellExecute = true
        });
    });
}

//app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
    app.Run();
else
    app.Run("http://localhost:5000");