using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Program.cs
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо сервіси
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            var app = builder.Build();

            // Налаштування middleware
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
