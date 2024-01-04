namespace server.Domain.DTOs;

public class AnswerDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public bool IsTrue { get; set; }
    public int QuestionId { get; set; }
}
