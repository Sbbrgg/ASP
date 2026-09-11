using System;
using System.Collections.Generic;

namespace Academy.Models;

public partial class Teacher
{
    public short TeacherId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte[]? Photo { get; set; }

    public DateOnly? WorkSince { get; set; }

    public decimal? Rate { get; set; }

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();
}
