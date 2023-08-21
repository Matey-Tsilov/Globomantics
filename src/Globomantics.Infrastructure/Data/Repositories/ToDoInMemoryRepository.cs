using Globomantics.Domain;
using System.Collections.Concurrent;

namespace Globomantics.Infrastructure.Data.Repositories;

public class ToDoInMemoryRepository<T> : IRepository<T> where T : Todo
{
    private ConcurrentDictionary<Guid, T> Items { get; } = new();
    public Task AddAsync(T item)
    {
        Items.TryAdd(item.Id, item);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<T>> AllAsync(Guid id)
    {
        var items = Items.Values.ToArray();
        return Task.FromResult<IEnumerable<T>>(items);
    }

    public Task<T> GetAsync(Guid id)
    {
        return Task.FromResult(Items[id]);
    }

    public Task<T> FindByAsync(string value)
    {
        var task = (from item in Items.Values
                    where item.Title == value
                    select item).FirstOrDefault();

        return Task.FromResult(task);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
