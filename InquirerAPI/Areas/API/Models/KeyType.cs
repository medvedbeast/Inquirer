namespace InquirerAPI.API.Models
{
    public class KeyType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static KeyType Generate(Website.Data.KeyType t)
        {
            var result = new KeyType
            {
                Id = t.Id,
                Name = t.Name
            };

            return result;
        }
    }
}
