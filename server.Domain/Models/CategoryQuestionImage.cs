namespace server.Domain.Models;

public class CategoryQuestionImage
{
    public int Id { get; set; }
    public string Path { get; set; }

    public int CategoryQuestionId { get; set; }
    public virtual CategoryQuestion CategoryQuestion { get; set; }
}
