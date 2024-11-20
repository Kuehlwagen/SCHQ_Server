using Microsoft.EntityFrameworkCore;
using SCHQ_Server.Models;
using SCHQ_Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Use MySQL database
builder.Services.AddDbContext<RelationsContext>(options =>
  options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

var app = builder.Build();

// Add / Update Database
using var scope = app.Services.CreateScope();
using var relationsContext = scope.ServiceProvider.GetService<RelationsContext>();
if (relationsContext!.Database.GetPendingMigrations().Any()) {
  /*
    Developer-PowerShell:
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet tool update --global dotnet-ef
    dotnet ef migrations add MigrationName
    dotnet ef migrations remove
    dotnet ef database update <previous-migration-name>
    dotnet ef database update 0
  */
  await relationsContext.Database.MigrateAsync();
}
await relationsContext.Database.EnsureCreatedAsync();

// Remove channels without password
try {
  Channel?[] channels = [.. relationsContext.Channels!.Where(c => string.IsNullOrEmpty(c.Password))];
  if (channels?.Length > 0) {
    relationsContext.RemoveRange(channels!);
    relationsContext.SaveChanges();
  }
} catch (Exception) { }

// Configure the HTTP request pipeline.
app.MapGrpcService<SCHQ_Service>();
app.MapGet("/", () => Results.Redirect("https://github.com/Kuehlwagen/Star-Citizen-Handle-Query", true, true));

app.Run();
