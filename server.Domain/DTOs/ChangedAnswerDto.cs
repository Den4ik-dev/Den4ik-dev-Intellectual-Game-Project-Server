namespace server.Domain.DTOs;
public class ChangedAnswerDto
{
  public string? Content { get; set; }
  public bool IsTrue { get; set; }
  public int QuestionId { get; set; }
}
