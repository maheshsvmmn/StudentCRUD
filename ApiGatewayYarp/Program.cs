using ApiGatewayYarp.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayYarp
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


            builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddHealthChecks();
            builder.Services.AddTransient<FilterRequestMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapReverseProxy(proxyPipeline =>
            {
                //proxyPipeline.UseFilterRequestMiddleware();
                //app.UseMiddleware<FilterRequestMiddleware>();
            });

            app.MapHealthChecks("health");

            app.Run();
        }
    }
}