using CitiesManager.WebAPI.DataBaseContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder
    .Services
    .AddDbContext<ApplicationDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
