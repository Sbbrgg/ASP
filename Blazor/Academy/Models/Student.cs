using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
	public class Student
	{
		[Key]
		public int stud_id { get; set; }
		public string last_name { get; set; }
		public string first_name { get; set; }
		public string? middle_name { get; set; }
		public DateTime? birth_date { get; set; }
		public string? email { get; set; }
		public string? phone { get; set; }
		[Column(TypeName = "IMAGE")]
		public byte[]? photo { get; set; }
		[ForeignKey(nameof(Group))]
		public int? group {  get; set; }

	}
}
