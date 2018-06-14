using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Option
    {
        public Option()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public bool IsCustom { get; set; }
        public int Index{ get; set; }
        public int QuestionId { get; set; }

        public Question Question { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
