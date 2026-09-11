using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Discipline
{
    public short DisciplineId { get; set; }

    public string DisciplineName { get; set; } = null!;

    public byte NumberOfLessons { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Discipline> DependentDiscipline1s { get; set; } = new List<Discipline>();

    public virtual ICollection<Direction> Directions { get; set; } = new List<Direction>();

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual ICollection<Discipline> DisciplinesNavigation { get; set; } = new List<Discipline>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Discipline> RequiredDiscipline1s { get; set; } = new List<Discipline>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
