namespace DotNetApi.Entities
{
  public class AppWithTasks
  {
    public string Id { get; set; }
    public required string Name { get; set; }
    public string Date { get; set; } = String.Empty;
    public string Edition { get; set; } = String.Empty;
    public string Server { get; set; } = String.Empty;
    public string ServerId { get; set; } = String.Empty;
    public AppTask[] Tasks { get; set; } = [];
  }

}
