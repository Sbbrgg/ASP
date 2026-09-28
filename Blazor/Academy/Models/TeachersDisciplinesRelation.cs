using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
	[PrimaryKey("teacher", "discipline")]
	public class TeachersDisciplinesRelation
	{
		[Column("teacher", TypeName = "SMALLINT")]
		[ForeignKey(nameof(Teachers))]
		public int teacher {  get; set; }

		[Column("discipline", TypeName = "SMALLINT")]
		[ForeignKey(nameof(Disciplines))]
		public int discipline { get; set; }

		//Navigation properies:
		public Teacher Teachers { get; set; }
		public Discipline Disciplines { get; set; }
	}
}
