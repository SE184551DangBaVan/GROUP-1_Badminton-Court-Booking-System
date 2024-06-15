using System;
using System.Collections.Generic;

namespace BadmintonBooking.Models;

public partial class Payment
{
    public int PId { get; set; }

    public decimal PAmount { get; set; }

    public DateTime PDateTime { get; set; }

    public int BId { get; set; }

    public virtual Booking BIdNavigation { get; set; } = null!;
}
