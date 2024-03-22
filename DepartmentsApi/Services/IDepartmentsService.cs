using DepartmentsApi.Models.Dtos;

namespace DepartmentsApi.Services
{
    public interface IDepartmentsService
    {
        List<DepartmentDto> GetDepartmentsAsync();
    }
}
