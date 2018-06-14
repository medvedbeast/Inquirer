using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.PublicAPI.Data
{
    public class Respondent
    {
        public Respondent()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public int? AssociatedUserId { get; set; }

        public User AssociatedUser { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
