// Data/UserSeeder.cs
using MongoDB.Driver;
using Prodify.Helpers;
using Prodify.Models;
using System;
using System.Threading.Tasks;

namespace Prodify.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(
            IMongoDatabase db,
            IPasswordHasher hasher)
        {
            var users = db.GetCollection<User>("users");

            var exists = await users.Find(u => u.Role == "superadmin")
                                    .AnyAsync();
            if (exists) return;

            // superadmin
            var now = DateTime.UtcNow;
            var superadmin = new User
            {
                Email = "superadmin@mail.com",
                Username = "superadmin",
                Name = "Super Administrator",
                Password = hasher.Hash("superadmin123"),
                Role = "superadmin",
                CreatedAt = now,
                UpdatedAt = now
            };

            // admin
            var admin = new User
            {
                Email = "admin@mail.com",
                Username = "admin",
                Name = "Administrator",
                Password = hasher.Hash("admin123"),
                Role = "admin",
                CreatedAt = now,
                UpdatedAt = now
            };

            // user
            var user = new User
            {
                Email = "user@mail.com",
                Username = "user",
                Name = "User",
                Password = hasher.Hash("user123"),
                Role = "user",
                CreatedAt = now,
                UpdatedAt = now
            };

            await users.InsertManyAsync(new[] { superadmin, admin, user });
            Console.WriteLine("User seeding completed successfully.");
        }
    }
}
