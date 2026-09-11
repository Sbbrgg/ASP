using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Attendance
{
    public int Student { get; set; }

    public long Lesson { get; set; }

    public bool Present { get; set; }

    public virtual Schedule LessonNavigation { get; set; } = null!;

    public virtual Student StudentNavigation { get; set; } = null!;
}
