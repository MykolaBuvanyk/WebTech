using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace CRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });

            var app = builder.Build();
            app.MapGet("/", () => Results.Redirect("/index.html"));
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Для wwwroot
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Додаємо тестові дані
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                if (!context.Users.Any())
                {
                    context.Users.Add(new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") });
                    var electronics = new Category { Name = "Electronics" };
                    context.Categories.Add(electronics);
                    context.SaveChanges();
                    context.Products.AddRange(
                        new Product { Name = "Laptop", Price = 999.99m, CategoryId = electronics.Id },
                        new Product { Name = "Phone", Price = 599.99m, CategoryId = electronics.Id }
                    );
                    context.SaveChanges();
                    context.ProductDetails.AddRange(
                        new ProductDetail { Description = "High performance", ProductId = 1 },
                        new ProductDetail { Description = "Latest model", ProductId = 2 }
                    );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
