namespace DotNetApi.Entities
{
  public class AppServer
  {
    public string Id { get; set; }
    public required string Name { get; set; }
    public string Date { get; set; } = String.Empty;
    public string Edition { get; set; } = String.Empty;
    public int Applications { get; set; } = 0;
    public int Tasks{ get; set; } = 0;
  }
}
