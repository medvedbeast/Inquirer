using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class QuestionType
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}
