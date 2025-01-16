using Kventin.Services.Infrastructure;
using Kventin.Services.Interfaces;
using Kventin.Services.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDataAccessServices(connectionString);

builder.Services.AddScoped<IFirstService, FirstService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // подключаем маршрутизацию на контроллеры
});

app.MapGet("/", () => "Hello World!");

app.Run();
