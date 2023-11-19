using ftreel.DATA;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

internal class Program
{
    private static void Main(string[] args)
    {

        /*var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        };*/

        var builder = WebApplication.CreateBuilder(args);

        var AnyOrigins = "_anyOrigin";

        // Database
        builder.Services.AddDbContext<AppDBContext>(options =>
        {
            var a = builder.Configuration.GetConnectionString("Database");
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
        });

        // Authentication cookie customization
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.SlidingExpiration = true;
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
        
        // Allow CORS
        builder.Services.AddCors( options => {
            options.AddPolicy(name: AnyOrigins,
                              policy => {
                                  policy
                                  .WithOrigins("http://localhost:5173")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials();
                              });
        });
        
        // Add services to the container.
        builder.Services.AddControllers();
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

        app.UseCors(AnyOrigins);
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseCookiePolicy(cookiePolicyOptions);

        app.MapControllers();

        app.Run();
    }

}
