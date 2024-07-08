using System.ComponentModel.DataAnnotations;

namespace demobadminton.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Username is required.")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters long.")]
        public string UserName { get; set; } = default!;

        [EmailAddress]
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="Confirm password is required.")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }= default!;


    }
}
