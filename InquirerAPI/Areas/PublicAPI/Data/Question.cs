using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            Options = new HashSet<Option>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Index { get; set; }
        public bool IsRequired { get; set; }
        public int SurveyId { get; set; }
        public int TypeId { get; set; }

        public Survey Survey { get; set; }
        public QuestionType Type { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
