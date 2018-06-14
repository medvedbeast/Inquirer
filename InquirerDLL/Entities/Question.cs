using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Question
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int? Index { get; set; }
        public bool? IsRequired { get; set; }
        public int? SurveyId { get; set; }
        public int? TypeId { get; set; }

        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Option> Options { get; set; }
    }
}
