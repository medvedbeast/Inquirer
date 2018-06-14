namespace InquirerAPI.API.Models
{
    public class Key
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public KeyType Type { get; set; }
        public int UserId { get; set; }

        public static Key Generate(Website.Data.Key k)
        {
            var result = new Key
            {
                Id = k.Id,
                Name = k.Name,
                Content = k.Content,
                Type = new KeyType
                {
                    Id = k.Type.Id,
                    Name = k.Type.Name
                },
                UserId = k.UserId
            };

            return result;
        }
    }
}
