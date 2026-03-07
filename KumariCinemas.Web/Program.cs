using KumariCinemas.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddControllersWithViews();
var provider = builder.Configuration["DataProvider"];
var connectionString = builder.Configuration.GetConnectionString("OracleDb");
var isOracleProvider = string.Equals(provider, "Oracle", StringComparison.OrdinalIgnoreCase)
    && !string.IsNullOrWhiteSpace(connectionString);

builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    if (isOracleProvider)
    {
        options.UseOracle(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("KumariCinemasDb");
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

    if (!isOracleProvider)
    {
        dbContext.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
