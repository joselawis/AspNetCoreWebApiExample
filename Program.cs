using CitiesManager.WebAPI.DataBaseContext;
using CitiesManager.WebAPI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        config.ApiVersionReader = new UrlSegmentApiVersionReader(); // Reads version number from request url at "apiVersion" constraint. /api/v1/cities
        // config.ApiVersionReader = new QueryStringApiVersionReader(); // Reads version from request query string called "api-version". /api/cities?api-version=1.0
        // config.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Reads version number from request header called "api-version". api-version: 1.0

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
    .AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
        options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });
        options.SwaggerDoc("v2", new OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });
    });

builder
    .Services
    .AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV"; // v1
        options.SubstituteApiVersionInUrl = true;
    });

// CORS: localhost:4200, localhost:4100
builder
    .Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(
            policyBuilder =>
                policyBuilder
                    .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .WithHeaders("Authorization", "origin", "accept", "content-type")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
        );
        options.AddPolicy(
            "4100Client",
            policyBuilder =>
                policyBuilder
                    .WithOrigins(
                        builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>()
                    )
                    .WithHeaders("Authorization", "origin", "accept")
                    .WithMethods("GET")
        );
    });

builder
    .Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
