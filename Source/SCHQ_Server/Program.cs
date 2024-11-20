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

// Configure the HTTP request pipeline.
app.MapGrpcService<SCHQ_Service>();
app.MapGet("/", () => Results.Redirect("https://github.com/Kuehlwagen/Star-Citizen-Handle-Query", true, true));

app.Run();
