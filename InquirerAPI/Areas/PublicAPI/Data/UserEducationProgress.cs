using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class UserEducationProgress
    {
        public UserEducationProgress()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
