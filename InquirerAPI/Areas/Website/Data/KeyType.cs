using System.Collections.Generic;

namespace InquirerAPI.Website.Data
{
    public class KeyType
    {
        public KeyType()
        {
            Keys = new HashSet<Key>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Key> Keys { get; set; }
    }
}
