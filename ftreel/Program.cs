using System.Text.Json.Serialization;
using ftreel.Constants;
using ftreel.DATA;
using ftreel.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ftreel.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        };

        var builder = WebApplication.CreateBuilder(args);

        const string anyOrigins = "_anyOrigin";
        var origin = builder.Configuration.GetConnectionString("Origin");

        // Database
        builder.Services.AddDbContext<AppDBContext>(options =>
        {
            var a = builder.Configuration.GetConnectionString("Database");
            options.UseLazyLoadingProxies();
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
        });
        
        builder.Services.Configure<UploadSettings>(builder.Configuration.GetSection(nameof(UploadSettings)));

        // Authentication cookie customization
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.SlidingExpiration = true;
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });
        
        // Allow CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: anyOrigins,
                policy =>
                {
                    policy
                        .WithOrigins(origin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        // https://code-maze.com/dotnet-how-to-solve-unable-to-resolve-service-for-a-type/
        builder.Services.AddScoped<AuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IStorageService, FileSystemStorageService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseCors(anyOrigins);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy(cookiePolicyOptions);

        app.MapControllers();
        
        // Create upload directory if not exists.
        if(!Directory.Exists(UploadPath.UPLOAD_FILE))
            Directory.CreateDirectory(UploadPath.UPLOAD_FILE);
        
        app.Run();
    }
}