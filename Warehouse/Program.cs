using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Auth;
using Warehouse.Behaviors;
using Warehouse.Caching;
using Warehouse.Data;
using Warehouse.Features.Products.Application.Mapping;
using Warehouse.Filters;

namespace Warehouse;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
        builder.Services.AddSingleton<JwtTokenService>();
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ProductMapper>();
        builder.Services.AddScoped<ProductListCache>();
        builder.Services.AddScoped<ProductListCacheFilter>();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            string? connection = builder.Configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = "Data Source=warehouse.db";
            }

            options.UseSqlite(connection);
        });

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserCommandBehavior<,>));

        JwtOptions jwt = new JwtOptions();
        builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwt);
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<AddApiHeaderAttribute>();
        });
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Warehouse API",
                Version = "v1"
            });
            OpenApiSecurityScheme bearer = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the token from POST /api/auth/login"
            };
            options.AddSecurityDefinition("Bearer", bearer);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        WebApplication app = builder.Build();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            DbSeeder.SeedAsync(db).GetAwaiter().GetResult();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse API v1");
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
