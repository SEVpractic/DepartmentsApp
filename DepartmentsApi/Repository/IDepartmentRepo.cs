using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Models.Entities;

namespace DepartmentsApi.Repository
{
    public interface IDepartmentRepo
    {
        Task<List<Department>> GetDepartmentsAsync();
        Task<bool> CreateDepartments(List<Department> departments);
		Task<bool> UpdateDepartments(List<Department> departments);
	}
}
