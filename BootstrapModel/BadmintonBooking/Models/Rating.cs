using System;
using System.Collections.Generic;

namespace BadmintonBooking.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int CourtId { get; set; }

    public string UserId { get; set; } = null!;

    public int? Rating1 { get; set; }

    public string? Review { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Court Court { get; set; } = null!;
}
