namespace server.Domain.Models;
public class User
{
  public int Id { get; set; }
  public string? Login { get; set; }
  public string? Password { get; set; }
  public int RoleId { get; set; }

  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
  public virtual ICollection<UserQuestion>? UserQuestions { get; set; }
  public virtual Role? Role { get; set; }
}