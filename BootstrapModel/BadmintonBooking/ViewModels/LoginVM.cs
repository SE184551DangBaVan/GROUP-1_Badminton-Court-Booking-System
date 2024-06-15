using System.ComponentModel.DataAnnotations;

namespace demobadminton.ViewModels
{
    public class LoginVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }

    }
}
