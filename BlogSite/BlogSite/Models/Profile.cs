namespace BlogSite.Models
    {
    public class Profile
        {
        public int Id { get; set; } // Primary key

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }
        }
    }
