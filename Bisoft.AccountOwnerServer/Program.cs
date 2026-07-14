
using Bisoft.AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Contracts;
using Repository;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Bisoft.AccountOwnerServer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.ConfigureLoggerService();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            //Automapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));


            builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("sqliteDefault")));
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


            //builder.Services.AddScoped(typeof(Entities.Helpers.ISortHelper<>), typeof(Entities.Helpers.SortHelper<>));
            //builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.ConfigureRepositoryWrapper();
            builder.Services.ConfigureSqliteContext(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseCors("CorsPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
