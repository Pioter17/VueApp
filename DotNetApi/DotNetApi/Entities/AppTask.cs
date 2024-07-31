namespace DotNetApi.Entities
{
  public class AppTask
  {
    public string Id { get; set; }
    public required string Name { get; set; }
    public string Date { get; set; } = String.Empty;
    public string Edition { get; set; } = String.Empty;
    public string Server { get; set; } = String.Empty;
    public string Application { get; set; } = String.Empty;
    public string ServerId { get; set; } = String.Empty;
    public string ApplicationId {  get; set; } = String.Empty;
  }
}
