using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register application services via extension method
builder.Services.AddApplicationServices(builder.Configuration);

// AddIdentityServices 
builder.Services.AddIdentityServices(builder.Configuration);

// cors
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


builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:DefaultConnection"] = "Data Source=app.db",
    ["JwtSettings:SecretKey"] = "your-super-secret-key-at-least-32-chars",
    ["JwtSettings:Issuer"] = "your-app",
    ["JwtSettings:Audience"] = "your-app",
    ["JwtSettings:ExpiryMinutes"] = "60",
    ["AllowedOrigins:0"] = "http://localhost:5000"
});


var app = builder.Build();

// Initialize the database with seed data
await app.InitializeDB();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


var embeddedProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "wwwroot");

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

// Auto open browser on start
var url = "http://localhost:5000";
Task.Run(async () =>
{
    await Task.Delay(1500);
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
});

app.Run(url);  // ← replace the existing app.Run() with this
