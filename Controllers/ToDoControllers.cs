using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using TodoList.Models;

namespace TodoList.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class ToDoItemsController : ControllerBase 
    { 
        private readonly DataContext _context;
        public ToDoItemsController(DataContext context) 
        { 
            _context = context; 
        }

    // GET: api/ToDoItems 
    [HttpGet] 
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems() 
    { 
        var toDoItems = await _context.ToDoItems 
        .Where(item => item.CompletedDate == null) 
        .ToListAsync();

    return Ok(toDoItems);
    }

    // GET: api/ToDoItems/5 
    [HttpGet("{id}")] 
    public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
    { 
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null) 
        { 
            return NotFound(); 
        }
        return toDoItem; 
    }

    // POST: api/ToDoItems 
    [HttpPost] 
    public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem toDoItem) 
    { 
        _context.ToDoItems.Add(toDoItem); 
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDoItem), 
        new 
        { 
            id = toDoItem.Id 
        }, toDoItem); 
    }

    // POST: api/ToDoItems/complete/5 
    [HttpPost("complete/{id}")] 
    public async Task<ActionResult> PostCompleteToDoItem(int id) 
    { 
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null) 
        { 
            return NotFound(); 
        }
        toDoItem.CompletedDate = DateTime.UtcNow; 
        _context.ToDoItems.Update(toDoItem); 
        await _context.SaveChangesAsync();
        return NoContent(); 
    }
}