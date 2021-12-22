using System;
using System.Collections.Generic;
namespace Backend.Models.Exam
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public string exam { get; set; }

        public int Duration { get; set; }

        public int NumberOfQuestion { get; set; }

    }

}
