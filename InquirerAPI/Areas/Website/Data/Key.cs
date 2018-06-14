using System;
using System.Collections.Generic;

namespace InquirerAPI.Website.Data
{
    public class Key
    {
        public Key()
        {
            Activities = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }

        public KeyType Type { get; set; }
        public User User { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
