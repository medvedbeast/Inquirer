using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerDLL.Entities
{
    public class Respondent
    {
        public int? Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public int? AssociatedUserId { get; set; }

        public IEnumerable<Answer> Answers { get; set; }
    }
}
