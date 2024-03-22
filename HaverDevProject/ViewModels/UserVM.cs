using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.ViewModels
{
    public class UserVM
    {
        public string ID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
    }
}
