using DepartmentsWeb.Models.Dto;

namespace DepartmentsWeb.Services
{
	public interface IDepartmentsService
	{
		Task<List<DepartmentDto>> GetFromApiAsync();
		Task<bool> SynchronizeDb(Stream fileStream);

    }
}