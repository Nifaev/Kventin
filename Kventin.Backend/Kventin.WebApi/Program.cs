using Kventin.Services.Infrastructure.Auth;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Auth;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Services;
using Microsoft.AspNetCore.CookiePolicy;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

var secretKey = new byte[32];
RandomNumberGenerator.Create().GetBytes(secretKey);

builder.Services.AddDataAccessServices(connectionString);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.Configure<JwtOption>(options =>
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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.MapControllers();


app.Run();
