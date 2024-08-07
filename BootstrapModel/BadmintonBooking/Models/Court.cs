﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BadmintonBooking.Models;

public partial class Court
{
    public int CoId { get; set; }
    [Required(ErrorMessage = "Court name is required")]
    [Display(Name = "Court name")]
    public string CoName { get; set; } = null!;

    public string? CoPath { get; set; }

    public bool CoStatus { get; set; }
    [Required(ErrorMessage = "Address is required")]
    [Display(Name = "Court Address")]
    public string CoAddress { get; set; } = null!;
    [Required(ErrorMessage = "Court information is required")]
    [Display(Name = "Court Information")]
    public string CoInfo { get; set; } = null!;
    [Required(ErrorMessage = "Court price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Court price must be greater than 0")]
    [Display(Name = "Court Price")]
    public double? CoPrice { get; set; }
    [NotMapped]
    [Display(Name = "Choose Image")]
    public IFormFile ImagePath { get; set; }
    public string UserId { get; set; } = null!;

    public string? CoBeneficiaryAccountName { get; set; }

    public string? CoBeneficiaryPayPalEmail { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CourtCondition> CourtConditions { get; set; } = new List<CourtCondition>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    public virtual AspNetUser User { get; set; } = null!;
}
