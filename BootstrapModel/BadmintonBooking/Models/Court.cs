using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadmintonBooking.Models;

public partial class Court
{
    public int CoId { get; set; }
    [Required(ErrorMessage = "Court name is required")]

    public string CoName { get; set; } = null!;

    public string? CoPath { get; set; }

    public bool CoStatus { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string CoAddress { get; set; } = null!;
    [Required(ErrorMessage = "Court information is required")]

    public string CoInfo { get; set; } = null!;
    [Required(ErrorMessage = "Court price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Court price must be greater than 0")]

    public double? CoPrice { get; set; }
    [NotMapped]
    [Display(Name = "Choose Image")]
    public IFormFile ImagePath { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    public virtual AspNetUser User { get; set; } = null!;
}
