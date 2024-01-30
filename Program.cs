using CitiesManager.WebAPI.DataBaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
        options.Filters.Add(new ConsumesAttribute("application/json"));
    })
    .AddXmlSerializerFormatters();

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
