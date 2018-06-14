using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Option
    {
        public int? Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public bool? IsCustom { get; set; }
        public int? QuestionId { get; set; }

        public IEnumerable<Answer> Answers { get; set; }
    }
}
