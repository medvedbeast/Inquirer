using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class UserLocation
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
