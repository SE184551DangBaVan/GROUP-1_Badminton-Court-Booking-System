using System.ComponentModel.DataAnnotations;

namespace demobadminton.ViewModels
{
    public class ForgetPasswordOrUserNameVM
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Token { get; set; }=default!;
        [Required(ErrorMessage ="Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get;set; } = default!;
        [Display(Name ="Confirm password")]
        [Compare("Password",ErrorMessage ="Password and confirm password do not match")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please enter confirm password")]
        public string ConfirmPassword { get; set; } = default!;

    }
}
