using DepartmentsWeb.Models.Dto;

namespace DepartmentsWeb.Services
{
	public interface IDepartmentsService
	{
		Task<List<DepartmentDto>> GetFromApiAsync();
	}
}