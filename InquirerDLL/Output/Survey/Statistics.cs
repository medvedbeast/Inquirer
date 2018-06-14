using System.Collections.Generic;

namespace InquirerDLL.Output.Survey
{
    public class Statistics
    {
        public List<Question> Questions { get; set; }


        public class Question
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int TypeId { get; set; }
            public int Maximum { get; set; }
            public List<Option> Options { get; set; }
        }

        public class Option
        {
            public int? Id { get; set; }
            public string Label { get; set; }
            public string Value { get; set; }
            public int Quantity { get; set; }
            public List<Answer> Answers { get; set; }
        }

        public class Answer
        {
            public string Content { get; set; }
            public int Quantity { get; set; }
        }

    }

}
