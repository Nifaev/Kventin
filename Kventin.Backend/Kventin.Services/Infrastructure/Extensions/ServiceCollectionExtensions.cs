using Kventin.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<KventinContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddApiAuthentication(this IServiceCollection services, byte[] secretKey)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["choco-cookies"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("Parent", policy => policy.RequireRole("Parent"))
                .AddPolicy("Teacher", policy => policy.RequireRole("Teacher"))
                .AddPolicy("Student", policy => policy.RequireRole("Student"))
                .AddPolicy("SuperUser", policy => policy.RequireRole("SuperUser"))
                .AddPolicy("AdminSchedule", policy => policy.RequireRole("AdminSchedule"))
                .AddPolicy("AdminGroups", policy => policy.RequireRole("AdminGroups"))
                .AddPolicy("AdminStudyProgress", policy => policy.RequireRole("AdminStudyProgress"))
                .AddPolicy("AdminAnnouncements", policy => policy.RequireRole("AdminAnnouncements"))
                .AddPolicy("AdminFinances", policy => policy.RequireRole("AdminFinances"))
                .AddPolicy("AdminPersonalAccounts", policy => policy.RequireRole("AdminPersonalAccounts"))
                .AddPolicy("AdminRegistration", policy => policy.RequireRole("AdminRegistration"));
        }
    }
}
