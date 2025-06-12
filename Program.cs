using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Prodify.Helpers;
using Prodify.Repositories;
using Prodify.Seeders;
using Prodify.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind settings + add Mongo
builder.Services.AddMongo(builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// REPOSITORY
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// SERVICE
builder.Services.AddScoped<IUserService, UserService>();

// HELPERS
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();


builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Prodify API", Version = "v1" });
    c.EnableAnnotations();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Masukkan JWT token dengan prefix **Bearer**\n\nExample: \"Bearer eyJhbGciOi…\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


// ——— SEEDER INVOCATION ———
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    // Ini memanggil seeder yang sudah Anda tulis:
    await UserSeeder.SeedAsync(db, hasher);
}

// Enforce auth middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
