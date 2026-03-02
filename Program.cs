using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Middleware;
using ToDoApp.Models;
using ToDoApp.Services;
using AutoMapper;

namespace ToDoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddProblemDetails();

            builder.Services.AddLogging();

            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("constr")
                )
             );

            builder.Services.AddScoped<ITodoServices, TodoServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler(); //Add this line for Exception

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}