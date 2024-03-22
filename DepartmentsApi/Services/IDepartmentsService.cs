using DepartmentsApi.Models.Dtos;

namespace DepartmentsApi.Services
{
    public interface IDepartmentsService
    {
        Task<List<DepartmentDto>> GetDepartmentsAsync();
    }
}
