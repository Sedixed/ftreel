using ftreel.DATA;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        // Authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.SlidingExpiration = true;
            });
        
        // Allow CORS

        builder.Services.AddCors( options => {
            options.AddPolicy(name: AnyOrigins,
                              policy => {
                                policy.WithOrigins("*")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
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

        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseCookiePolicy(cookiePolicyOptions);

        app.MapControllers();

        app.Run();
    }

}
