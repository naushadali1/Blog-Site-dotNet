using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.ViewModel
    {
    public class ProfileVM
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password{ get; set; }
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not Match")]
        public string ConfirmPassword{ get; set; }

        public string Bio { get; set; }

        public IFormFile? Image { get; set; }
        }
    }
