using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
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
    string sortBy = "Name", 
    bool sortDesc = false 
)
    {
      var serversQuery = _context.Servers.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        serversQuery = serversQuery.Where(s => s.Name.ToLower().Contains(serverName.ToLower()));
      }

      serversQuery = sortBy switch
      {
        "name" => sortDesc ? serversQuery.OrderByDescending(s => s.Name) : serversQuery.OrderBy(s => s.Name),
        "date" => sortDesc ? serversQuery.OrderByDescending(s => s.Date) : serversQuery.OrderBy(s => s.Date),
        _ => sortDesc ? serversQuery.OrderByDescending(s => s.Name) : serversQuery.OrderBy(s => s.Name),
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

    [HttpGet("export-servers")]
    public async Task<IActionResult> ExportTasksToExcel()
    {
      var servers = await _context.Servers.ToListAsync();

      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Servers");

      worksheet.Cells[1, 1].Value = "Id";
      worksheet.Cells[1, 2].Value = "Name";
      worksheet.Cells[1, 3].Value = "Date";
      worksheet.Cells[1, 4].Value = "Edition";

      using (var range = worksheet.Cells[1, 1, 1, 4])
      {
        range.Style.Font.Bold = true;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
      }

      for (int i = 0; i < servers.Count; i++)
      {
        var server = servers[i];
        worksheet.Cells[i + 2, 1].Value = server.Id;
        worksheet.Cells[i + 2, 2].Value = server.Name;
        worksheet.Cells[i + 2, 3].Value = server.Date;
        worksheet.Cells[i + 2, 4].Value = server.Edition;
      }

      worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

      var stream = new MemoryStream();
      package.SaveAs(stream);
      stream.Position = 0;

      string excelName = $"Servers-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }

    [HttpGet("export-page")]
    public async Task<ActionResult> ExportPage(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string sortBy = "Name",
    bool sortDesc = false
)
    {
      var serversQuery = _context.Servers.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        serversQuery = serversQuery.Where(a => a.Name.ToLower().Contains(serverName.ToLower()));
      }

      serversQuery = sortBy switch
      {
        "name" => sortDesc ? serversQuery.OrderByDescending(a => a.Name) : serversQuery.OrderBy(a => a.Name),
        "date" => sortDesc ? serversQuery.OrderByDescending(a => a.Date) : serversQuery.OrderBy(a => a.Date),
        _ => sortDesc ? serversQuery.OrderByDescending(a => a.Name) : serversQuery.OrderBy(a => a.Name),
      };

      if (pageNumber <= 0 || pageSize <= 0)
      {
        return BadRequest("Page number and page size must be greater than zero.");
      }

      var servers = await serversQuery
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

      using (var package = new ExcelPackage())
      {
        var worksheet = package.Workbook.Worksheets.Add("Page Data");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Date";
        worksheet.Cells[1, 4].Value = "Edition";

        for (int i = 0; i < servers.Count; i++)
        {
          var server = servers[i];
          worksheet.Cells[i + 2, 1].Value = server.Id;
          worksheet.Cells[i + 2, 2].Value = server.Name;
          worksheet.Cells[i + 2, 3].Value = server.Date;
          worksheet.Cells[i + 2, 4].Value = server.Edition;
        }

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"Servers_Page_{pageNumber}.xlsx";
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
      }
    }

  }
}

