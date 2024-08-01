using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", // Allow specific origin
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            builder.Services.AddDistributedMemoryCache();

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

            app.UseCors("AllowSpecificOrigin");
            app.MapGet("/", async () =>
            {
                var filePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "index.html");
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                return Results.Content(Encoding.UTF8.GetString(fileBytes), "text/html");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
