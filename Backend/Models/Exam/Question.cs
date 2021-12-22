using System.Collections.Generic;
namespace Backend.Models.Exam
{
    public class Question
    {
        public int Id { get; set; }
        public string question { get; set; }

        public string RightAnswerCode { get; set; }

        public IEnumerable<Answer> Answers { get; set; }

    }
}
