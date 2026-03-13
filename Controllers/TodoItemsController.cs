using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // All endpoints require authentication
public class TodoItemsController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoItemsController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // GET: api/TodoItems — accessible to all authenticated users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var items = await _todoService.GetAllAsync();
        return Ok(items.Select(ItemToDTO));
    }

    // GET: api/TodoItems/{id} — accessible to all authenticated users
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(string id)
    {
        var todoItem = await _todoService.GetByIdAsync(id);
        if (todoItem == null) return NotFound();
        return ItemToDTO(todoItem);
    }

    // POST: api/TodoItems — admin only
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            Name = todoDTO.Name,
            IsComplete = todoDTO.IsComplete
        };

        var created = await _todoService.CreateAsync(todoItem);
        return CreatedAtAction(nameof(GetTodoItem), new { id = created.Id }, ItemToDTO(created));
    }

    // PUT: api/TodoItems/{id} — admin only
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> PutTodoItem(string id, TodoItemDTO todoDTO)
    {
        var existing = await _todoService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Name = todoDTO.Name;
        existing.IsComplete = todoDTO.IsComplete;

        await _todoService.UpdateAsync(id, existing);
        return NoContent();
    }

    // DELETE: api/TodoItems/{id} — admin only
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteTodoItem(string id)
    {
        var todoItem = await _todoService.GetByIdAsync(id);
        if (todoItem == null) return NotFound();

        await _todoService.RemoveAsync(id);
        return NoContent();
    }

    private static TodoItemDTO ItemToDTO(TodoItem item) =>
        new TodoItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete
        };
}
