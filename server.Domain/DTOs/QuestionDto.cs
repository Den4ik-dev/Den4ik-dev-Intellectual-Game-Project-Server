namespace server.Domain.DTOs;
public class QuestionDto
{
  public int Id { get; set; }
  public string? Content { get; set; }
  public int ImageId { get; set; }
  public int CategoryQuestionId { get; set; }
  public ICollection<int>? AnswersIds { get; set; }
}