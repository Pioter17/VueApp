using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

    [HttpGet("paginated-servers")]
    public async Task<ActionResult> GetAllServers(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string sortBy = "Name", // Domyślne sortowanie
    bool sortDesc = false // Domyślnie rosnąco
)
    {
      var serversQuery = _context.Servers.AsQueryable();

      // Filtrowanie
      if (!string.IsNullOrWhiteSpace(serverName))
      {
        serversQuery = serversQuery.Where(s => s.Name.ToLower().Contains(serverName.ToLower()));
      }

      // Sortowanie
      serversQuery = sortBy switch
      {
        "name" => sortDesc ? serversQuery.OrderByDescending(s => s.Name) : serversQuery.OrderBy(s => s.Name),
        "date" => sortDesc ? serversQuery.OrderByDescending(s => s.Date) : serversQuery.OrderBy(s => s.Date),
        _ => sortDesc ? serversQuery.OrderByDescending(s => s.Name) : serversQuery.OrderBy(s => s.Name), // Domyślne sortowanie
      };

      if (pageSize == -1)
      {
        var allServers = await serversQuery.ToListAsync();
        var response = new
        {
          TotalItems = allServers.Count(),
          TotalPages = 1,
          CurrentPage = 1,
          PageSize = allServers.Count,
          Servers = allServers
        };
        return Ok(response);
      }
      else
      {
        if (pageNumber <= 0 || pageSize <= 0)
        {
          return BadRequest("Page number and page size must be greater than zero.");
        }

        var totalServers = await serversQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalServers / (double)pageSize);

        var servers = await serversQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var response = new
        {
          TotalItems = totalServers,
          TotalPages = totalPages,
          CurrentPage = pageNumber,
          PageSize = pageSize,
          Servers = servers
        };

        return Ok(response);
      }
    }


    [HttpGet]
    public async Task<ActionResult<List<AppServer>>> getAllServers()
    {
      return Ok(await _context.Servers.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<List<AppServer>>> AddServer([FromBody] AppServer server)
    {
      var existingServer = await _context.Servers.FirstOrDefaultAsync(s => s.Name == server.Name);
      if (existingServer != null)
      {
        return Conflict(new { message = "Server with the same name already exists." });
      }

      _context.Add(server);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.CountAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<AppServer>>> UpdateServer(AppServer updatedServer)
    {
      var dbServer = await _context.Servers.FindAsync(updatedServer.Id);
      if (dbServer == null)
        return NotFound("Server not found.");

      var existingServer = await _context.Servers.FirstOrDefaultAsync(s => s.Name == updatedServer.Name);
      if (existingServer != null)
      {
        return Conflict(new { message = "Server with the same name already exists." });
      }

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

