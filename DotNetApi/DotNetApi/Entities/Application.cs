namespace DotNetApi.Entities
{
  public class Application
  {
    public string Id { get; set; }
    public required string Name { get; set; }
    public string Date { get; set; } = String.Empty;
    public string Edition { get; set; } = String.Empty;
    public string Server { get; set; } = String.Empty;
    public string ServerId { get; set; } = String.Empty;
    public int Tasks { get; set; } = 0;
  }

}
