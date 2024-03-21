namespace HaverDevProject.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    namespace HaverDevProject.ViewModels
    {
        public class CreateUserVM
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Roles")]
            public List<string> SelectedRoles { get; set; }
        }
    }

}
