using DepartmentsApi.Models.Dtos;

namespace DepartmentsApi.Services
{
    public interface IDepartmentsService
    {
        Task<List<DepartmentDto>> GetDepartments();
        Task<List<DepartmentDto>> CreteOrUpdateAsync(List<DepartmentDto> departmentDtos);
    }
}
