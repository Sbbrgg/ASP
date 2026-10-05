using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
	public class Student:Human
	{
		[Key]
		public int stud_id { get; set; }

		[Required]
		[ForeignKey(nameof(Group))]
		public int group { get; set; }

		//			Navigation properties:
		[NotMapped]
		public Group Group { get; set; }
	}
}
