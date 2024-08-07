using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  public class TaskController : ControllerBase
  {
    private readonly DataContext _context;

    public TaskController(DataContext context)
    {
      _context = context;
    }

    [HttpGet("paginated-tasks")]
    public async Task<ActionResult> GetTasks(
      int pageNumber = 1,
      int pageSize = 10,
      string serverName = "",
      string applicationName = "",
      string taskName = ""
    )
    {
      var tasksQuery = _context.Tasks.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        tasksQuery = tasksQuery.Where(t => t.Server.ToLower().Contains(serverName.ToLower()));
      }

      if (!string.IsNullOrWhiteSpace(applicationName))
      {
        tasksQuery = tasksQuery.Where(t => t.Application.ToLower().Contains(applicationName.ToLower()));
      }

      if (!string.IsNullOrWhiteSpace(taskName))
      {
        tasksQuery = tasksQuery.Where(t => t.Name.ToLower().Contains(taskName.ToLower()));
      }

      if (pageSize == -1)
      {
        var allTasks = await tasksQuery.ToListAsync();
        var response = new
        {
          TotalItems = allTasks.Count(),
          TotalPages = 1,
          CurrentPage = 1,
          PageSize = allTasks.Count,
          Tasks = allTasks
        };
        return Ok(response);
      }
      else
      {
        if (pageNumber <= 0 || pageSize <= 0)
        {
          return BadRequest("Page number and page size must be greater than zero.");
        }

        var totalTasks = await tasksQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

        var tasks = await tasksQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var response = new
        {
          TotalItems = totalTasks,
          TotalPages = totalPages,
          CurrentPage = pageNumber,
          PageSize = pageSize,
          Tasks = tasks
        };

        return Ok(response);
      }
    }

    [HttpGet]
    public async Task<ActionResult<List<AppTask>>> getAllTasks()
    {
      return Ok(await _context.Tasks.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<List<AppTask>>> AddTask([FromBody]AppTask task)
    {
      if (task == null)
      {
        return BadRequest();
      }
      _context.Add(task);
      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.CountAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<AppTask>>> UpdateTask(AppTask updatedTask)
    {
      var dbTask = await _context.Tasks.FindAsync(updatedTask.Id);
      if (dbTask == null)
        return NotFound("Task not found.");

      dbTask.Name = updatedTask.Name;
      dbTask.Date = updatedTask.Date;
      dbTask.Edition = updatedTask.Edition;
      dbTask.Server = updatedTask.Server;
      dbTask.ServerId = updatedTask.ServerId;
      dbTask.ApplicationId = updatedTask.ApplicationId;
      dbTask.Application = updatedTask.Application;

      await _context.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<List<AppTask>>> DeleteTask(string id)
    {
      var dbTask = await _context.Tasks.FindAsync(id);
      if (dbTask == null)
        return NotFound("Task not found.");

      _context.Tasks.Remove(dbTask);
      await _context.SaveChangesAsync();

      return Ok();
    }
  }
}

