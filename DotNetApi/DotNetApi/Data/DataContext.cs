using DotNetApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<AppTask> Tasks { get; set; }
    public DbSet<Application> Apps { get; set; }
    public DbSet<AppServer> Servers { get; set; }
  }
}
