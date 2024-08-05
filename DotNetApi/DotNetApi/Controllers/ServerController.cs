using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  public class ServerController : ControllerBase
  {
    private readonly DataContext _context;

    public ServerController(DataContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppServer>>> GetAllServers()
    {
      var servers = await _context.Servers.ToListAsync();
      return Ok(servers);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AppServer>> GetServerById(string id)
    {
      var server = await _context.Servers.FindAsync(id);
      if (server == null)
      {
        return NotFound("Server not found");
      }
      return Ok(server);
    }

    [HttpPost]
    public async Task<ActionResult<List<AppServer>>> AddServer([FromBody] AppServer server)
    {
      _context.Add(server);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<AppServer>>> UpdateServer(AppServer updatedServer)
    {
      var dbServer = await _context.Servers.FindAsync(updatedServer.Id);
      if (dbServer == null)
        return NotFound("Server not found.");

      dbServer.Name = updatedServer.Name;
      dbServer.Edition = updatedServer.Edition;

      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [HttpDelete]
    public async Task<ActionResult<List<AppServer>>> DeleteServer(string id)
    {
      var dbServer = await _context.Servers.FindAsync(id);
      if (dbServer == null)
        return NotFound("Server not found.");

      _context.Servers.Remove(dbServer);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }
  }
}

