namespace server.Domain.Models;
public class Image
{
  public int Id { get; set; }
  public string? Path { get; set; }

  public int QuestionId { get; set; }
  public virtual Question? Question { get; set; } 
}