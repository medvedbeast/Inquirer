using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class UserOccupation
    {
        public UserOccupation()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
