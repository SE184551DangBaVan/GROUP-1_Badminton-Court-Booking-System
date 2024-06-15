using System;
using System.Collections.Generic;

namespace BadmintonBooking.Models;

public partial class Court
{
    public int CoId { get; set; }

    public string CoName { get; set; } = null!;

    public string? CoPath { get; set; }

    public bool CoStatus { get; set; }

    public string CoAddress { get; set; } = null!;

    public string CoInfo { get; set; } = null!;

    public double? CoPrice { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}
