using System;
using System.Collections.Generic;

namespace InquirerAPI.Website.Data
{
    public class Activity
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime OccuredOn { get; set; }
        public string Content { get; set; }
        public int ExternalUserId { get; set; }
        public int KeyId { get; set; }
        public int UserId { get; set; }

        public Key Key { get; set; }
        public User User { get; set; }
    }
}
