using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todoItems;

    public TodoService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _todoItems = database.GetCollection<TodoItem>(settings.Value.TodoItemsCollectionName);
    }

    public async Task<List<TodoItem>> GetAllAsync() =>
        await _todoItems.Find(_ => true).ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(string id) =>
        await _todoItems.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<TodoItem> CreateAsync(TodoItem todoItem)
    {
        await _todoItems.InsertOneAsync(todoItem);
        return todoItem;
    }

    public async Task UpdateAsync(string id, TodoItem updatedItem) =>
        await _todoItems.ReplaceOneAsync(x => x.Id == id, updatedItem);

    public async Task RemoveAsync(string id) =>
        await _todoItems.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> ExistsAsync(string id) =>
        await _todoItems.Find(x => x.Id == id).AnyAsync();
}
