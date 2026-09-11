using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Direction
{
    public byte DirectionId { get; set; }

    public string? DirectionName { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();
}
