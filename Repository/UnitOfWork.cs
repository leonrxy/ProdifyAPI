
using System.Collections.Concurrent;
using MongoDB.Driver;
using Prodify.Models;

namespace Prodify.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;

    IUserRepository User { get; }
    IProductRepository Product { get; }
}


public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoDatabase _db;
    private readonly ConcurrentDictionary<string, object> _repos = new();
    private IUserRepository? _users;
    private IProductRepository? _products;


    public UnitOfWork(IMongoDatabase db)
    {
        _db = db;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var name = typeof(T).FullName!;
        return (IRepository<T>)_repos.GetOrAdd(name, _ =>
            new GenericRepository<T>(_db)
        );
    }

    public IUserRepository User => _users ??= new UserRepository(_db);
    public IProductRepository Product => _products ??= new ProductRepository(_db);

    public void Dispose() { /* nothing else to dispose */ }
}