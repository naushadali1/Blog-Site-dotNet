using System.ComponentModel;

namespace BlogSite.ViewModel
    {
    public class PostVM
        {

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        [DisplayName("Cover image for blog post")]
        public IFormFile Image { get; set; }
        public string Slug { get; set; }
        }
    }
