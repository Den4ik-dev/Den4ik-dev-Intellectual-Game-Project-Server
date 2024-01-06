namespace server.Domain.DTOs;

public class UserQuestionDetailsDto
{
    public string ImagePath { get; set; }
    public string QuestionContent { get; set; }
    public string[] Answers { get; set; }
    public int AnswerNumber { get; set; }
    public int TrueAnswerNumber { get; set; }
}
