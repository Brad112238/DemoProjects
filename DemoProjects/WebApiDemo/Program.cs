
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Application;
using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;
using WebApiDemo.Services;

namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<TestContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("TestDb"));
            });

            builder.Services.AddScoped<IUserCreditService, UserCreditService>();
            builder.Services.AddScoped<IEcpayService, EcpayService>();
            builder.Services.AddScoped<UserCreditApplication>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
