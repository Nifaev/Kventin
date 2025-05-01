using System.Reflection;
using System.Security.Cryptography;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Infrastructure.Tools;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Interfaces.Tools;
using Kventin.Services.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

var secretKey = new byte[32];
RandomNumberGenerator.Create().GetBytes(secretKey);

builder.Services.AddDataAccessServices(connectionString);

builder.Services.AddEndpointsApiExplorer();

builder
    .Services
    .AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

// 🟢 Добавляем CORS для Vue с Vite
builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            "AllowVueApp",
            policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173") // Адрес Vite-сервера для разработки
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); // Если используете авторизацию через куки
            }
        );
    });

builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder
    .Services
    .Configure<JwtOption>(options =>
    {
        builder.Configuration.GetSection("JwtOptions").Bind(options);
        options.SecretKey = secretKey;
    });

builder.Services.AddApiAuthentication(secretKey);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🟢 Активируем CORS
app.UseCors("AllowVueApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
    }
);

// 🟢 Раздача собранного фронтенда (Vue)
var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "kventin-frontend", "dist");
if (Directory.Exists(frontendPath))
{
    app.UseDefaultFiles();
    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(frontendPath),
            RequestPath = ""
        }
    );
    app.MapFallbackToFile("index.html"); // Поддержка SPA-маршрутизации
}

app.MapControllers();

app.Run();
