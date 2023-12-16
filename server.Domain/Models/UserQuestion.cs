namespace server.Domain.Models;
public class UserQuestion
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public int QuestionId { get; set; }
  public bool Complete { get; set; } = false;
  public int AnswerNumber { get; set; }

  public DateTime UserQuestionExpiryTime { get; set; }

  public virtual User? User { get; set; }
  public virtual Question? Question { get; set; }
}