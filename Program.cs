using CitiesManager.WebAPI.DataBaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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
    .AddApiVersioning(config =>
    {
        // config.ApiVersionReader = new UrlSegmentApiVersionReader(); // Reads version number from request url at "apiVersion" constraint. /api/v1/cities
        // config.ApiVersionReader = new QueryStringApiVersionReader(); // Reads version from request query string called "api-version". /api/cities?api-version=1.0
        config.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Reads version number from request header called "api-version". api-version: 1.0

        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
    });

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
