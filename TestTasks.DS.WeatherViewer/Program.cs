using TestTasks.DS.WeatherViewer.Models.DBEntities;
using TestTasks.DS.WeatherViewer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddScoped<WeatherViewerDbContext>();
builder.Services.AddScoped<WeatherArchiveRecordsRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();

app.Run();