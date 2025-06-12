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

            // Cek jika sudah ada user Admin
            var exists = await users.Find(u => u.role == "Admin")
                                    .AnyAsync();
            if (exists) return;

            // superadmin
            var now = DateTime.UtcNow;
            var superadmin = new User
            {
                email = "superadmin@mail.com",
                username = "superadmin",
                name = "Super Administrator",
                password = hasher.Hash("superadmin123"),
                role = "Superadmin",
                created_at = now,
                updated_at = now
            };

            // admin
            var admin = new User
            {
                email = "admin@mail.com",
                username = "admin",
                name = "Administrator",
                password = hasher.Hash("admin123"),
                role = "Admin",
                created_at = now,
                updated_at = now
            };

            // user
            var user = new User
            {
                email = "user@mail.com",
                username = "user",
                name = "User",
                password = hasher.Hash("user123"),
                role = "User",
                created_at = now,
                updated_at = now
            };

            await users.InsertManyAsync(new[] { superadmin, admin, user });
            Console.WriteLine("User seeding completed successfully.");
        }
    }
}
