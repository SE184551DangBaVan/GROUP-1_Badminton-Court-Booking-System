using System.ComponentModel.DataAnnotations;

namespace BadmintonBooking.ViewModels
{
    public class ChangePasswordViewModel
    {

        
        

        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = default!;

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;


        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm new password is required.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = default!;

    }
}
