namespace server.Domain.DTOs;
public class UserQuestionDto
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public int CategoryQuestionId { get; set; }
  public int QuestionId { get; set; }
  public string? QuestionContent { get; set; }
  public bool Complete { get; set; }
  public int AnswerNumber { get; set; }
}
