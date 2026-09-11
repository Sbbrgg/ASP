using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Grade
{
    public int Student { get; set; }

    public long Lesson { get; set; }

    public byte? Grade1 { get; set; }

    public byte? Grade2 { get; set; }

    public virtual Schedule LessonNavigation { get; set; } = null!;

    public virtual Student StudentNavigation { get; set; } = null!;
}
