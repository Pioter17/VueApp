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
    public async Task<ActionResult<List<Application>>> GetAllApps()
    {
      var apps = await _context.Apps.ToListAsync();
      return Ok(apps);
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
    public async Task<ActionResult<List<Application>>> AddApplication([FromBody] Application app)
    {
      _context.Add(app);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Application>>> UpdateApplication(Application updatedApp)
    {
      var dbApp = await _context.Apps.FindAsync(updatedApp.Id);
      if (dbApp == null)
        return NotFound("Hero not found.");

      dbApp.Name = updatedApp.Name;
      dbApp.Date = updatedApp.Date;
      dbApp.Edition = updatedApp.Edition;
      dbApp.Server = updatedApp.Server;
      dbApp.ServerId = updatedApp.ServerId;
      dbApp.Tasks = updatedApp.Tasks;

      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [HttpDelete]
    public async Task<ActionResult<List<Application>>> DeleteApplication(string id)
    {
      var dbApp = await _context.Apps.FindAsync(id);
      if (dbApp == null)
        return NotFound("Hero not found.");

      _context.Apps.Remove(dbApp);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }
  }
}

