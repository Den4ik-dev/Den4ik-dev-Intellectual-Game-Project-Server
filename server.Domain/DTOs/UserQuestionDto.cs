namespace server.Domain.DTOs;
public class UserQuestionDto
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public int CategoryQuestionId { get; set; }
  public string? QuestionContent { get; set; }
  public string[]? Answers { get; set; }
  public string? ImagePath { get; set; }
}