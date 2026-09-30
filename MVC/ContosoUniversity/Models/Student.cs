namespace ContosoUniversity.Models
{
	public class Student
	{
		public int ID { get; set; }
		public string last_name { get; set; }
		public string first_name { get; set; }
		public DateTime EnrollmentDate { get; set; }

		//Navigation properties
		public ICollection<Enrollment>? Enrollments { get; set; }
	}
}
