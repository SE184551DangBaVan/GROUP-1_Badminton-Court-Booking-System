using System.ComponentModel.DataAnnotations;

namespace demobadminton.ViewModels
{
    public class RegisterVM
    {
        [EmailAddress]
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="Confirm password is required.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }= default!;


    }
}
