using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Collector
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? IsOpen { get; set; }
        public int? SurveyId { get; set; }
    }
}
