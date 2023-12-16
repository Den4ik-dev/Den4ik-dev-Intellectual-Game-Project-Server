namespace server.Domain.DTOs;
public class AddedAnswerDto
{
  public string? Content { get; set; }
  public bool IsTrue { get; set; }
  public int QuestionId { get; set; }
}
