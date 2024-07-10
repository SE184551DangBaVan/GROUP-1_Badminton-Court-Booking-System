using System;
using System.Collections.Generic;

namespace BadmintonBooking.Models;

public partial class UserActiveStatus
{
    public string Id { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual AspNetUser IdNavigation { get; set; } = null!;
}
