using System;
using System.Collections.Generic;

namespace InquirerAPI.Website.Data
{
    public class User
    {
        public User()
        {
            Activities = new HashSet<Activity>();
            Keys = new HashSet<Key>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime LastSeenOn { get; set; }

        public ICollection<Activity> Activities { get; set; }
        public ICollection<Key> Keys { get; set; }
    }
}
