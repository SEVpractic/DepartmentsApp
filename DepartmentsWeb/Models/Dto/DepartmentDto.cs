using System.ComponentModel.DataAnnotations;

namespace DepartmentsWeb.Models.Dto
{
	public class DepartmentDto
	{
		public long? DepartmentId { get; set; }
		public string? Name { get; set; }
		[Required]
		public bool IsActive { get; set; }
		public int? ParentId { get; set; }
	}
}
