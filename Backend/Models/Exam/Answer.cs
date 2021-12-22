namespace Backend.Models.Exam
{

    public class Answer
    {
        public int Id { get; set; }
        public string answer { get; set; }
        public string AnswerCode { get; set; }

        public int QuestionId { get; set; }
    }
}
