using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;
using WebApplication3.Service;
using WebApplication3.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация и проверка файла настроек
try
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}
catch (Exception ex)
{
    throw new InvalidOperationException("Error loading configuration", ex);
}

// База данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// Регистрация сервисов
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CourseSearchService>(); // Добавлено

// Конфигурация IIS и FormOptions
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1073741824;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824;
});

// Аутентификация
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
    });

// Дополнительные сервисы
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Конвейер обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application terminated unexpectedly: {ex}");
    throw;
}