using CitiesManager.WebAPI.DataBaseContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder
    .Services
    .AddDbContext<ApplicationDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder
    .Services
    .AddSwaggerGen(
        options => options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"))
    );

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
