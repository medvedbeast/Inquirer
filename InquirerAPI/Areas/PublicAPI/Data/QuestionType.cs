using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
