using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace BadmintonBooking.ViewModels
{
    public class ResendEmailConfirmationVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = default!;
      
    }
}
