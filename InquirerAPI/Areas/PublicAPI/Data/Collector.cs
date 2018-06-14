using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Collector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOpen { get; set; }
        public int SurveyId { get; set; }

        public Survey Survey { get; set; }
    }
}
