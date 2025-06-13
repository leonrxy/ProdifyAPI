// Data/ProductSeeder.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Prodify.Models;

namespace Prodify.Seeders
{
    public static class ProductSeeder
    {
        public static async Task SeedAsync(IMongoDatabase db)
        {
            var products = db.GetCollection<Product>("products");

            // jika sudah ada produk, skip seeding
            var any = await products.Find(_ => true).AnyAsync();
            if (any) return;

            // cari admin user (role 'admin' atau 'superadmin')
            var users = db.GetCollection<User>("users");
            var adminUser = await users.Find(u => u.Role == "admin").FirstOrDefaultAsync()
                            ?? await users.Find(u => u.Role == "superadmin").FirstOrDefaultAsync();

            AdminInfo adminInfo = null;
            if (adminUser != null)
            {
                adminInfo = new AdminInfo
                {
                    Id = adminUser.Id,
                    Email = adminUser.Email,
                    Username = adminUser.Username
                };
            }

            var now = DateTime.UtcNow;
            var seedList = new List<Product>
            {
                new Product
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Basic Widget",
                    Description = "Widget berkualitas tinggi untuk keperluan sehari-hari.",
                    Price = 19999m,
                    PhotoUrl = "https://example.com/images/widget1.jpg",
                    DisplayStart = now,
                    DisplayEnd = now.AddMonths(1),
                    Status = ProductStatus.active,
                    Stock = 100,
                    Admin = adminInfo,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Product
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Premium Gadget",
                    Description = "Gadget eksklusif dengan fitur canggih.",
                    Price = 49999m,
                    PhotoUrl = "https://example.com/images/gadget1.jpg",
                    DisplayStart = now,
                    DisplayEnd = now.AddMonths(2),
                    Status = ProductStatus.active,
                    Stock = 50,
                    Admin = adminInfo,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Product
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Collector’s Edition",
                    Description = "Edisi terbatas untuk kolektor sejati.",
                    Price = 129999m,
                    PhotoUrl = "https://example.com/images/collectors.jpg",
                    DisplayStart = now,
                    DisplayEnd = now.AddMonths(3),
                    Status = ProductStatus.draft,
                    Stock = 10,
                    Admin = adminInfo,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            };

            await products.InsertManyAsync(seedList);
            Console.WriteLine("Product seeding completed successfully.");
        }
    }
}
