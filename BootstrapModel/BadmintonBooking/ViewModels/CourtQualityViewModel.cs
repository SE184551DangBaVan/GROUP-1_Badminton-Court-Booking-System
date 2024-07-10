using System.ComponentModel.DataAnnotations;

namespace BadmintonBooking.ViewModels
{
    public class CourtQualityViewModel
    {
        [Required]
        [Range(1, 5)]
        [Display(Name = "Court surface condition")]
        public int CdSurfaceCondition { get; set; }
        [Required]
        [Range(1, 5)]
        [Display(Name = "Court lightning condition")]
        public int CdLightningCondition { get; set; }
        [Required]
        [Range(1, 5)]
        [Display(Name = "Court net condition")]
        public int CdNetCondition { get; set; }
        [Required]
        [Range(1, 5)]
        [Display(Name = "Court cleanliness condition")]
        public int CdCleanlinessCondtion { get; set; }
        [Required]
        [Range(1, 5)]
        public int CdOverallCondition { get; set; }
        [Display(Name = "Additional Notes")]
        public string? CdNotes { get; set; }
    }
}
