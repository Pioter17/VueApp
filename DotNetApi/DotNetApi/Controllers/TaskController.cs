using DotNetApi.Data;
using DotNetApi.Entities;
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
      string taskName = "",
      string sortBy = "Name", 
      bool sortDesc = false 
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

      tasksQuery = sortBy switch
      {
        "name" => sortDesc ? tasksQuery.OrderByDescending(t => t.Name) : tasksQuery.OrderBy(t => t.Name),
        "date" => sortDesc ? tasksQuery.OrderByDescending(t => t.Date) : tasksQuery.OrderBy(t => t.Date),
        "edition" => sortDesc ? tasksQuery.OrderByDescending(t => t.Edition) : tasksQuery.OrderBy(t => t.Edition),
        _ => sortDesc ? tasksQuery.OrderByDescending(t => t.Name) : tasksQuery.OrderBy(t => t.Name), // Domyślne sortowanie
      };

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

      var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == task.Name);
      if (existingTask != null)
      {
        return Conflict(new { message = "Task with the same name already exists." });
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

      var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == updatedTask.Name);
      if (existingTask != null)
      {
        return Conflict(new { message = "Task with the same name already exists." });
      }

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

    [HttpGet("export-tasks")]
    public async Task<IActionResult> ExportTasksToExcel()
    {
      var tasks = await _context.Tasks.ToListAsync();

      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Tasks");

      worksheet.Cells[1, 1].Value = "Id";
      worksheet.Cells[1, 2].Value = "Name";
      worksheet.Cells[1, 3].Value = "Date";
      worksheet.Cells[1, 4].Value = "Edition";
      worksheet.Cells[1, 5].Value = "Server";
      worksheet.Cells[1, 6].Value = "ServerId";
      worksheet.Cells[1, 7].Value = "Application";
      worksheet.Cells[1, 8].Value = "ApplicationId";

      using (var range = worksheet.Cells[1, 1, 1, 8])
      {
        range.Style.Font.Bold = true;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
      }

      for (int i = 0; i < tasks.Count; i++)
      {
        var task = tasks[i];
        worksheet.Cells[i + 2, 1].Value = task.Id;
        worksheet.Cells[i + 2, 2].Value = task.Name;
        worksheet.Cells[i + 2, 3].Value = task.Date;
        worksheet.Cells[i + 2, 4].Value = task.Edition;
        worksheet.Cells[i + 2, 5].Value = task.Server;
        worksheet.Cells[i + 2, 6].Value = task.ServerId;
        worksheet.Cells[i + 2, 7].Value = task.Application;
        worksheet.Cells[i + 2, 8].Value = task.ApplicationId;
      }

      worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

      var stream = new MemoryStream();
      package.SaveAs(stream);
      stream.Position = 0;

      string excelName = $"Tasks-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }

    [HttpGet("export-page")]
    public async Task<ActionResult> ExportPage(
    int pageNumber = 1,
    int pageSize = 10,
    string serverName = "",
    string applicationName = "",
    string taskName = "",
    string sortBy = "Name",
    bool sortDesc = false
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

      tasksQuery = sortBy switch
      {
        "name" => sortDesc ? tasksQuery.OrderByDescending(a => a.Name) : tasksQuery.OrderBy(a => a.Name),
        "date" => sortDesc ? tasksQuery.OrderByDescending(a => a.Date) : tasksQuery.OrderBy(a => a.Date),
        _ => sortDesc ? tasksQuery.OrderByDescending(a => a.Name) : tasksQuery.OrderBy(a => a.Name),
      };

      if (pageNumber <= 0 || pageSize <= 0)
      {
        return BadRequest("Page number and page size must be greater than zero.");
      }

      var tasks = await tasksQuery
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
        worksheet.Cells[1, 7].Value = "Application";
        worksheet.Cells[1, 8].Value = "ApplicationId";

        for (int i = 0; i < tasks.Count; i++)
        {
          var task = tasks[i];
          worksheet.Cells[i + 2, 1].Value = task.Id;
          worksheet.Cells[i + 2, 2].Value = task.Name;
          worksheet.Cells[i + 2, 3].Value = task.Date;
          worksheet.Cells[i + 2, 4].Value = task.Edition;
          worksheet.Cells[i + 2, 5].Value = task.Server;
          worksheet.Cells[i + 2, 6].Value = task.ServerId;
          worksheet.Cells[i + 2, 7].Value = task.Application;
          worksheet.Cells[i + 2, 8].Value = task.ApplicationId;
        }

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"Tasks_Page_{pageNumber}.xlsx";
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
      }
    }

  }
}

