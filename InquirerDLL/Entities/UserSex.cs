using System;
using System.Collections.Generic;
using System.Text;

namespace InquirerDLL.Entities
{
    public class UserSex
    {
        public bool? Id { get; set; }
        public string Name { get; set; }

        IEnumerable<User> Users { get; set; }
    }
}
