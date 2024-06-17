using System.ComponentModel.DataAnnotations;

namespace BadmintonBooking.ViewModels
{
    public class CreateRoleViewModel
    {

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
