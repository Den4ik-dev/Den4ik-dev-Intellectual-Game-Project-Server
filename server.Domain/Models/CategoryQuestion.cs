namespace server.Domain.Models;

public class CategoryQuestion
{
    public int Id { get; set; }
    public string? Title { get; set; }

    public virtual ICollection<Question>? Questions { get; set; }
}
