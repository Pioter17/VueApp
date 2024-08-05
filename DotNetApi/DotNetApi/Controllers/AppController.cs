using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  public class AppController : ControllerBase
  {
    private readonly DataContext _context;

    public AppController(DataContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllApps(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string applicationName = "")
    {
      // Filtruj aplikacje na podstawie przekazanych kryteriów
      var appsQuery = _context.Apps.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        appsQuery = appsQuery.Where(a => a.Server.ToLower().Contains(serverName.ToLower()));
      }

      if (!string.IsNullOrWhiteSpace(applicationName))
      {
        appsQuery = appsQuery.Where(a => a.Name.ToLower().Contains(applicationName.ToLower()));
      }

      if (pageSize == -1)
      {
        var allApps = await appsQuery.ToListAsync();
        var response = new
        {
          TotalItems = allApps.Count(),
          TotalPages = 1,
          CurrentPage = 1,
          PageSize = allApps.Count,
          Applications = allApps
        };
        return Ok(response);
      }
      else
      {
        if (pageNumber <= 0 || pageSize <= 0)
        {
          return BadRequest("Page number and page size must be greater than zero.");
        }

        var totalApps = await appsQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalApps / (double)pageSize);

        var apps = await appsQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var response = new
        {
          TotalItems = totalApps,
          TotalPages = totalPages,
          CurrentPage = pageNumber,
          PageSize = pageSize,
          Applications = apps
        };

        return Ok(response);
      }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Application>> GetAppById(string id)
    {
      var app = await _context.Apps.FindAsync(id);
      if (app == null)
      {
        return NotFound("Application not found");
      }
      return Ok(app);
    }

    [HttpPost]
    public async Task<ActionResult<List<Application>>> AddApplication([FromBody] AppWithTasks app)
    {
      var tasks = await _context.Tasks.ToListAsync();
      foreach (var newTask in app.Tasks) 
      {
        tasks.ForEach(task => 
        {
          if (task.Id == newTask.Id)
          {
            task.Application = app.Name;
            task.ApplicationId = app.Id;
          }
        });
      }

      var newApp = new Application()
      {
        Name = app.Name,
        Tasks = app.Tasks.Length,
        Id = app.Id,
        Date = app.Date,
        Edition = app.Edition,
        Server = app.Server,
        ServerId = app.ServerId,
      };

      _context.Add(newApp);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Application>>> UpdateApplication(AppWithTasks updatedApp)
    {
      var dbApp = await _context.Apps.FindAsync(updatedApp.Id);
      if (dbApp == null)
        return NotFound("Application not found.");

      var tasks = await _context.Tasks.ToListAsync();
      tasks.ForEach(task =>
      {
        if (task.ApplicationId == updatedApp.Id)
        {
          task.Application = "";
          task.ApplicationId = "";
        }
      });

      foreach (var newTask in updatedApp.Tasks)
      {
        tasks.ForEach(task =>
        {
          if (task.Id == newTask.Id)
          {
            task.Application = updatedApp.Name;
            task.ApplicationId = updatedApp.Id;
          }
        });
      }

      dbApp.Name = updatedApp.Name;
      dbApp.Date = updatedApp.Date;
      dbApp.Edition = updatedApp.Edition;
      dbApp.Server = updatedApp.Server;
      dbApp.ServerId = updatedApp.ServerId;
      dbApp.Tasks = updatedApp.Tasks.Length;

      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [HttpDelete]
    public async Task<ActionResult<List<Application>>> DeleteApplication(string id)
    {
      var dbApp = await _context.Apps.FindAsync(id);
      if (dbApp == null)
        return NotFound("Application not found.");

      var tasks = await _context.Tasks.ToListAsync();
      tasks.ForEach(task =>
      {
        if(task.ApplicationId == id)
        {
          task.Application = "";
          task.ApplicationId = "";
        }
      });

      _context.Apps.Remove(dbApp);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }
  }
}

