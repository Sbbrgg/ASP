using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Salary
{
    public long PaymentId { get; set; }

    public short Teacher { get; set; }

    public decimal Accrued { get; set; }

    public bool Received { get; set; }

    public virtual Teacher TeacherNavigation { get; set; } = null!;
}
