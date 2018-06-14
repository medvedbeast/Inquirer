using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Answer
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public int? OptionId { get; set; }
        public int? QuestionId { get; set; }
        public int? RespondentId { get; set; }
    }
}
