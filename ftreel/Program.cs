using ftreel.DATA;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {

        /*var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        };*/

        var builder = WebApplication.CreateBuilder(args);

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
