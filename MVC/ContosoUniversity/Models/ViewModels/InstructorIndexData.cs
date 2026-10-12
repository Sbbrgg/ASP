namespace ContosoUniversity.Models.ViewModels
{
	public class InstructorIndexData
	{
		public PaginatedList<Instructor> Instructors { get; set; }
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<Enrollment> Enrollments { get; set; }
	}
}