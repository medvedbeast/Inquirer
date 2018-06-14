using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Report
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int CreatorId { get; set; }

        public User Creator { get; set; }
        public User User { get; set; }
    }
}
