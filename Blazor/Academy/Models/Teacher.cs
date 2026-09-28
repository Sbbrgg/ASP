using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
	public class Teacher : Human
	{
		[Key]
		[Column("teacher_id", TypeName = "SMALLINT")]
		public int teacher_id { get; set; }

		public DateOnly work_since { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "SMALLMONEY")]
		public decimal rate { get; set; }

		[NotMapped]
		public string Experience
		{
			get
			{
				DateOnly today = DateOnly.FromDateTime(DateTime.Today);

				if (work_since > today) return "0 месяцев";

				int months = ((today.Year - work_since.Year) * 12) + today.Month - work_since.Month;
				if (today.Day < work_since.Day) months--;
				if (months < 0) months = 0;

				if (months < 12)
				{
					// Склонение слова "месяц"
					int lastDigit = months % 10;
					int lastTwoDigits = months % 100;
					string suffix = "месяцев";

					if (lastTwoDigits < 11 || lastTwoDigits > 14)
					{
						if (lastDigit == 1) suffix = "месяц";
						else if (lastDigit >= 2 && lastDigit <= 4) suffix = "месяца";
					}

					return $"{months} {suffix}";
				}
				else
				{
					int years = months / 12;
					int remainingMonths = months % 12;

					// Склонение слова "год"
					int lastDigit = years % 10;
					int lastTwoDigits = years % 100;
					string yearSuffix = "лет";

					if (lastTwoDigits < 11 || lastTwoDigits > 14)
					{
						if (lastDigit == 1) yearSuffix = "год";
						else if (lastDigit >= 2 && lastDigit <= 4) yearSuffix = "года";
					}

					if (remainingMonths == 0)
						return $"{years} {yearSuffix}";

					return $"{years} {yearSuffix} {remainingMonths} мес.";
				}
			}
		}

		//Navigation properties
		public ICollection<TeachersDisciplinesRelation> DisciplinesRelations { get; set; } = default!;
	}
}
