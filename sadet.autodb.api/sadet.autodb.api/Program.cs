using Microsoft.EntityFrameworkCore;
using sadet.autodb.api.Context;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => { options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IGamesRepository, GamesRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddControllers();

var app = builder.Build();
//app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();