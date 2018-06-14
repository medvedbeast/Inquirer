using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? OptionId { get; set; }
        public int? QuestionId { get; set; }
        public int RespondentId { get; set; }

        public Option Option { get; set; }
        public Question Question { get; set; }
        public Respondent Respondent { get; set; }
    }
}
