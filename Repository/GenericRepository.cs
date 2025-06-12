// Infrastructure/Repositories/GenericRepository.cs
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly IMongoCollection<T> _col;

    public GenericRepository(IMongoDatabase db)
    {
        var name = typeof(T).Name.ToLower() + "s";
        _col = db.GetCollection<T>(name);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var objId = ObjectId.Parse(id);
        return await _col.Find(Builders<T>.Filter.Eq("_id", objId))
                      .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _col.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _col.Find(predicate).ToListAsync();

    public async Task CreateAsync(T entity) =>
        await _col.InsertOneAsync(entity);

    public async Task UpdateAsync(T entity)
    {
        var idProp = typeof(T).GetProperty("id")?.GetValue(entity)?.ToString()
                      ?? throw new InvalidOperationException("Entity must have an Id property");

        var objId = ObjectId.Parse(idProp);

        await _col.ReplaceOneAsync(
            Builders<T>.Filter.Eq("_id", objId),
            entity
        );
    }

    public async Task DeleteAsync(string id)
    {
        var objId = ObjectId.Parse(id);
        await _col.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objId));
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
        await _col.Find(predicate).AnyAsync();
}
