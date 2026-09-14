using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
	public class Group
	{
		[Key]
		public int group_id {  get; set; }
		public string group_name { get; set; }
		[Column(TypeName = "TINYINT")]
		public byte? direction { get; set; }
		[Column(TypeName = "TINYINT")]
		public byte? weekdays { get; set; }
		public TimeSpan? start_time	{ get; set; }
		public DateTime? start_date { get; set; }
	}
}
