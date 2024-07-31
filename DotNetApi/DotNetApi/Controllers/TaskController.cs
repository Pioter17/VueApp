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

    [HttpGet]
    public async Task<ActionResult<List<AppTask>>> GetAllTasks()
    {
      var tasks = await _context.Tasks.Select(t => new
      {
        t.Id,
        t.Name,
        t.Date, // Obsłuż `NULL` w Date
        Edition = t.Edition ?? null, // Obsłuż `NULL` w EditionDate
        t.Server,
        Application = t.Application ?? null,
        t.ServerId,
        ApplicationId = t.ApplicationId ?? null,
      }).ToListAsync(); ;
      return Ok(tasks);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AppTask>> GetTaskById(string id)
    {
      var task = await _context.Tasks.FindAsync(id);
      if (task == null)
      {
        return NotFound("Task not found");
      }
      return Ok(task);
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

      return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<List<AppTask>>> UpdateTask(AppTask updatedTask)
    {
      var dbTask = await _context.Tasks.FindAsync(updatedTask.Id);
      if (dbTask == null)
        return NotFound("Hero not found.");

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
        return NotFound("Hero not found.");

      _context.Tasks.Remove(dbTask);
      await _context.SaveChangesAsync();

      return Ok();
    }
  }
}

