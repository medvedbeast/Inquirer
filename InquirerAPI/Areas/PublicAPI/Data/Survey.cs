using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class Survey
    {
        public Survey()
        {
            Collectors = new HashSet<Collector>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }
        public bool IsAuthenticationRequired { get; set; }
        public int CreatorId { get; set; }

        public User Creator { get; set; }
        public ICollection<Collector> Collectors { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
