using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class Report
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public int? CreatorId { get; set; }
    }
}
