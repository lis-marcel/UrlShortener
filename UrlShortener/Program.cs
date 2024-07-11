using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using UrlShortener.Database;
using UrlShortener.Service;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<DbStorageContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AppDbConnection")));

            builder.Services.AddScoped<UrlShorteningService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SessionService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".UrlShortener.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

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

            app.UseSession();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.MapGet("/", async () =>
            {
                var filePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "index.html");
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                return Results.Content(Encoding.UTF8.GetString(fileBytes), "text/html");
            });

            app.UseMiddleware<SessionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
