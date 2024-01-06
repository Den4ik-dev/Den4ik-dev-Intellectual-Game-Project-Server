namespace server.Domain.Models;

public class Question
{
    public int Id { get; set; }
    public string? Content { get; set; }

    public virtual QuestionImage? Image { get; set; }
    public int CategoryQuestionId { get; set; }
    public virtual CategoryQuestion? CategoryQuestion { get; set; }
    public virtual ICollection<Answer>? Answers { get; set; }
}
