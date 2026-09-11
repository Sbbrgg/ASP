using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class DaysOff
{
    public DateOnly Date { get; set; }

    public byte Holiday { get; set; }

    public virtual Holiday HolidayNavigation { get; set; } = null!;
}
