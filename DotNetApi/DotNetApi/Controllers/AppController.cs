using DotNetApi.Data;
using DotNetApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;

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

    [HttpGet("paginated-apps")]
    public async Task<ActionResult> GetApps(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string applicationName = "",
    string sortBy = "Name", 
    bool sortDesc = false 
)
    {
      var appsQuery = _context.Apps.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        appsQuery = appsQuery.Where(a => a.Server.ToLower().Contains(serverName.ToLower()));
      }

      if (!string.IsNullOrWhiteSpace(applicationName))
      {
        appsQuery = appsQuery.Where(a => a.Name.ToLower().Contains(applicationName.ToLower()));
      }

      appsQuery = sortBy switch
      {
        "name" => sortDesc ? appsQuery.OrderByDescending(a => a.Name) : appsQuery.OrderBy(a => a.Name),
        "date" => sortDesc ? appsQuery.OrderByDescending(a => a.Date) : appsQuery.OrderBy(a => a.Date),
        _ => sortDesc ? appsQuery.OrderByDescending(a => a.Name) : appsQuery.OrderBy(a => a.Name), // Domyślne sortowanie
      };

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
    public async Task<ActionResult<List<Application>>> getAllApps()
    {
      return Ok(await _context.Apps.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<List<Application>>> AddApplication([FromBody] AppWithTasks app)
    {
      var existingApp = await _context.Apps.FirstOrDefaultAsync(a => a.Name == app.Name);
      if (existingApp != null)
      {
        return Conflict(new { message = "Application with the same name already exists." });
      }

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

      return Ok(await _context.Apps.CountAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Application>>> UpdateApplication(AppWithTasks updatedApp)
    {
      var dbApp = await _context.Apps.FindAsync(updatedApp.Id);
      if (dbApp == null)
        return NotFound("Application not found.");

      var existingApp = await _context.Apps.FirstOrDefaultAsync(a => a.Name == updatedApp.Name);
      if (existingApp != null)
      {
        return Conflict(new { message = "Application with the same name already exists." });
      }

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

    [HttpGet("export-applications")]
    public async Task<IActionResult> ExportTasksToExcel()
    {
      var apps = await _context.Apps.ToListAsync();

      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Applications");

      worksheet.Cells[1, 1].Value = "Id";
      worksheet.Cells[1, 2].Value = "Name";
      worksheet.Cells[1, 3].Value = "Date";
      worksheet.Cells[1, 4].Value = "Edition";
      worksheet.Cells[1, 5].Value = "Server";
      worksheet.Cells[1, 6].Value = "ServerId";

      using (var range = worksheet.Cells[1, 1, 1, 6])
      {
        range.Style.Font.Bold = true;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
      }

      for (int i = 0; i < apps.Count; i++)
      {
        var app = apps[i];
        worksheet.Cells[i + 2, 1].Value = app.Id;
        worksheet.Cells[i + 2, 2].Value = app.Name;
        worksheet.Cells[i + 2, 3].Value = app.Date;
        worksheet.Cells[i + 2, 4].Value = app.Edition;
        worksheet.Cells[i + 2, 5].Value = app.Server;
        worksheet.Cells[i + 2, 6].Value = app.ServerId;
      }

      worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

      var stream = new MemoryStream();
      package.SaveAs(stream);
      stream.Position = 0;

      string excelName = $"Applications-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }

    [HttpGet("export-page")]
    public async Task<ActionResult> ExportPage(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string applicationName = "",
    string sortBy = "Name",
    bool sortDesc = false
)
    {
      var appsQuery = _context.Apps.AsQueryable();

      if (!string.IsNullOrWhiteSpace(serverName))
      {
        appsQuery = appsQuery.Where(a => a.Server.ToLower().Contains(serverName.ToLower()));
      }

      if (!string.IsNullOrWhiteSpace(applicationName))
      {
        appsQuery = appsQuery.Where(a => a.Name.ToLower().Contains(applicationName.ToLower()));
      }

      appsQuery = sortBy switch
      {
        "name" => sortDesc ? appsQuery.OrderByDescending(a => a.Name) : appsQuery.OrderBy(a => a.Name),
        "date" => sortDesc ? appsQuery.OrderByDescending(a => a.Date) : appsQuery.OrderBy(a => a.Date),
        _ => sortDesc ? appsQuery.OrderByDescending(a => a.Name) : appsQuery.OrderBy(a => a.Name),
      };

      if (pageNumber <= 0 || pageSize <= 0)
      {
        return BadRequest("Page number and page size must be greater than zero.");
      }

      var apps = await appsQuery
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
        worksheet.Cells[1, 5].Value = "Server";
        worksheet.Cells[1, 6].Value = "ServerId";

        for (int i = 0; i < apps.Count; i++)
        {
          var app = apps[i];
          worksheet.Cells[i + 2, 1].Value = app.Id;
          worksheet.Cells[i + 2, 2].Value = app.Name;
          worksheet.Cells[i + 2, 3].Value = app.Date;
          worksheet.Cells[i + 2, 4].Value = app.Edition;
          worksheet.Cells[i + 2, 5].Value = app.Server;
          worksheet.Cells[i + 2, 6].Value = app.ServerId;
        }

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"Apps_Page_{pageNumber}.xlsx";
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
      }
    }

  }
}

