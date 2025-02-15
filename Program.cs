
using Api_1.Entity;
using Api_1.Outherize;
using Api_1.Repository;
using Api_1.Validation;
using FluentValidation.AspNetCore;
using HangfireBasicAuthenticationFilter;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Api_1.Settings;
using Microsoft.AspNetCore.Identity;

namespace Api_1;

public class Program
{
    [Obsolete]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDependencies(builder.Configuration);

        // تسجيل FluentValidation
        builder.Services.AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<studentValidator>();
        });

        builder.Services.AddDbContext<EducationalPlatformContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseHangfireDashboard();
        }
        //var looger = app.Logger;
        //app.Use(async (context, next) =>
        //{
        //    looger.LogInformation("prosaa request");
        //    await next(context);
        //    looger.LogInformation("prosses reaspons");
        //});


        app.UseHttpsRedirection();

        app.UseHangfireDashboard("/jobs", new DashboardOptions
        {
            Authorization =
            [
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
                    Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                }
            ],

            DashboardTitle = "Education Platform",
            //IsReadOnlyFunc = (DashboardContext conext) => true
        });

        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        RecurringJob.AddOrUpdate("SendNewCoursNotification", () => notificationService.SendNewCoursNotification(), Cron.Daily);

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        

        app.MapControllers();
       
        //app.UseRouting();

        app.UseRateLimiter();


        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.Run();
    }
}
