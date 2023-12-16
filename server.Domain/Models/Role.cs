namespace server.Domain.Models;
public class Role
{
  public int Id { get; set; }
  public string Name { get; set; }

  public virtual ICollection<User>? Users { get; set; }
}

public static class RoleTypes
{
  public const string USER = "user";
  public const string ADMIN = "admin";
}