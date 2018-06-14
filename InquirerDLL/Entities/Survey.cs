using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Survey
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsOpen { get; set; }
        public bool? IsAuthenticationRequired { get; set; }
        public int? CreatorId { get; set; }

        public IEnumerable<Collector> Collectors { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
