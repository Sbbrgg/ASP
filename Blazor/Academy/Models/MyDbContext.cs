using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Academy.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<DaysOff> DaysOffs { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JAU235M\\SQLEXPRESS;Database=PV_521_Import;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => new { e.Student, e.Lesson });

            entity.ToTable("Attendance");

            entity.Property(e => e.Student).HasColumnName("student");
            entity.Property(e => e.Lesson).HasColumnName("lesson");
            entity.Property(e => e.Present).HasColumnName("present");

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Lesson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Schedule");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Students");
        });

        modelBuilder.Entity<DaysOff>(entity =>
        {
            entity.HasKey(e => e.Date).HasName("PK__DaysOFF__D9DE21FC5ACA9273");

            entity.ToTable("DaysOFF");

            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Holiday).HasColumnName("holiday");

            entity.HasOne(d => d.HolidayNavigation).WithMany(p => p.DaysOffs)
                .HasForeignKey(d => d.Holiday)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DO_Holidays");
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.Property(e => e.DirectionId).HasColumnName("direction_id");
            entity.Property(e => e.DirectionName)
                .HasMaxLength(50)
                .HasColumnName("direction_name");

            entity.HasMany(d => d.Disciplines).WithMany(p => p.Directions)
                .UsingEntity<Dictionary<string, object>>(
                    "DisciplinesDirectionsRelation",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DisciplinesDirectionsRelation_Disciplines"),
                    l => l.HasOne<Direction>().WithMany()
                        .HasForeignKey("Direction")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DisciplinesDirectionsRelation_Directions"),
                    j =>
                    {
                        j.HasKey("Direction", "Discipline");
                        j.ToTable("DisciplinesDirectionsRelation");
                        j.IndexerProperty<byte>("Direction").HasColumnName("direction");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                    });
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.Property(e => e.DisciplineId)
                .ValueGeneratedNever()
                .HasColumnName("discipline_id");
            entity.Property(e => e.DisciplineName)
                .HasMaxLength(150)
                .HasColumnName("discipline_name");
            entity.Property(e => e.NumberOfLessons).HasColumnName("number_of_lessons");

            entity.HasMany(d => d.DependentDiscipline1s).WithMany(p => p.Disciplines)
                .UsingEntity<Dictionary<string, object>>(
                    "DependentDiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("DependentDiscipline1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DependentDisciplines_Disciplines1"),
                    l => l.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DependentDisciplines_Disciplines"),
                    j =>
                    {
                        j.HasKey("Discipline", "DependentDiscipline1");
                        j.ToTable("DependentDisciplines");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                        j.IndexerProperty<short>("DependentDiscipline1").HasColumnName("dependent_discipline");
                    });

            entity.HasMany(d => d.Disciplines).WithMany(p => p.DependentDiscipline1s)
                .UsingEntity<Dictionary<string, object>>(
                    "DependentDiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DependentDisciplines_Disciplines"),
                    l => l.HasOne<Discipline>().WithMany()
                        .HasForeignKey("DependentDiscipline1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DependentDisciplines_Disciplines1"),
                    j =>
                    {
                        j.HasKey("Discipline", "DependentDiscipline1");
                        j.ToTable("DependentDisciplines");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                        j.IndexerProperty<short>("DependentDiscipline1").HasColumnName("dependent_discipline");
                    });

            entity.HasMany(d => d.DisciplinesNavigation).WithMany(p => p.RequiredDiscipline1s)
                .UsingEntity<Dictionary<string, object>>(
                    "RequiredDiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RequiredDisciplines_Disciplines"),
                    l => l.HasOne<Discipline>().WithMany()
                        .HasForeignKey("RequiredDiscipline1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RequiredDisciplines_Disciplines1"),
                    j =>
                    {
                        j.HasKey("Discipline", "RequiredDiscipline1");
                        j.ToTable("RequiredDisciplines");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                        j.IndexerProperty<short>("RequiredDiscipline1").HasColumnName("required_discipline");
                    });

            entity.HasMany(d => d.RequiredDiscipline1s).WithMany(p => p.DisciplinesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "RequiredDiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("RequiredDiscipline1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RequiredDisciplines_Disciplines1"),
                    l => l.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RequiredDisciplines_Disciplines"),
                    j =>
                    {
                        j.HasKey("Discipline", "RequiredDiscipline1");
                        j.ToTable("RequiredDisciplines");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                        j.IndexerProperty<short>("RequiredDiscipline1").HasColumnName("required_discipline");
                    });
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => new { e.Student, e.Discipline });

            entity.Property(e => e.Student).HasColumnName("student");
            entity.Property(e => e.Discipline).HasColumnName("discipline");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Grade).HasColumnName("grade");

            entity.HasOne(d => d.DisciplineNavigation).WithMany(p => p.Exams)
                .HasForeignKey(d => d.Discipline)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exams_Disciplines");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Exams)
                .HasForeignKey(d => d.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exams_Students");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => new { e.Student, e.Lesson }).HasName("PK_Grades_1");

            entity.Property(e => e.Student).HasColumnName("student");
            entity.Property(e => e.Lesson).HasColumnName("lesson");
            entity.Property(e => e.Grade1).HasColumnName("grade_1");
            entity.Property(e => e.Grade2).HasColumnName("grade_2");

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.Lesson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Schedule");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Students");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.GroupId)
                .ValueGeneratedNever()
                .HasColumnName("group_id");
            entity.Property(e => e.Direction).HasColumnName("direction");
            entity.Property(e => e.GroupName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("group_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartTime)
                .HasPrecision(0)
                .HasColumnName("start_time");
            entity.Property(e => e.Weekdays).HasColumnName("weekdays");

            entity.HasOne(d => d.DirectionNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Direction)
                .HasConstraintName("FK_Groups_Directions");

            entity.HasMany(d => d.Disciplines).WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "CompleteDiscipline",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CompleteDisciplines_Disciplines"),
                    l => l.HasOne<Group>().WithMany()
                        .HasForeignKey("Group")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CompleteDisciplines_Groups"),
                    j =>
                    {
                        j.HasKey("Group", "Discipline");
                        j.ToTable("CompleteDisciplines");
                        j.IndexerProperty<int>("Group").HasColumnName("group");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                    });
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("PK__Holidays__253884EAF289280A");

            entity.Property(e => e.HolidayId).HasColumnName("holiday_id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.HolidayName)
                .HasMaxLength(150)
                .HasColumnName("holiday_name");
            entity.Property(e => e.Month).HasColumnName("month");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Salary__ED1FC9EAD616C394");

            entity.ToTable("Salary");

            entity.Property(e => e.PaymentId)
                .ValueGeneratedNever()
                .HasColumnName("payment_id");
            entity.Property(e => e.Accrued)
                .HasColumnType("smallmoney")
                .HasColumnName("accrued");
            entity.Property(e => e.Received).HasColumnName("received");
            entity.Property(e => e.Teacher).HasColumnName("teacher");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_Teachers");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.LessonId);

            entity.ToTable("Schedule");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Discipline).HasColumnName("discipline");
            entity.Property(e => e.Group).HasColumnName("group");
            entity.Property(e => e.Spent).HasColumnName("spent");
            entity.Property(e => e.Teacher).HasColumnName("teacher");
            entity.Property(e => e.Time)
                .HasPrecision(0)
                .HasColumnName("time");

            entity.HasOne(d => d.DisciplineNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Discipline)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Disciplines");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Group)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Groups");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Teachers");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudId);

            entity.Property(e => e.StudId).HasColumnName("stud_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Group).HasColumnName("group");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Photo)
                .HasColumnType("image")
                .HasColumnName("photo");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK_Students_Groups");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Photo)
                .HasColumnType("image")
                .HasColumnName("photo");
            entity.Property(e => e.Rate)
                .HasColumnType("smallmoney")
                .HasColumnName("rate");
            entity.Property(e => e.WorkSince).HasColumnName("work_since");

            entity.HasMany(d => d.Disciplines).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeachersDisciplinesRelation",
                    r => r.HasOne<Discipline>().WithMany()
                        .HasForeignKey("Discipline")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TeachersDisciplinesRelation_Disciplines"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("Teacher")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TeachersDisciplinesRelation_Teachers"),
                    j =>
                    {
                        j.HasKey("Teacher", "Discipline");
                        j.ToTable("TeachersDisciplinesRelation");
                        j.IndexerProperty<short>("Teacher").HasColumnName("teacher");
                        j.IndexerProperty<short>("Discipline").HasColumnName("discipline");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
