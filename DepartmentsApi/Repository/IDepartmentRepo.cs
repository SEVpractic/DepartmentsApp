using DepartmentsApi.Models.Entities;

namespace DepartmentsApi.Repository
{
    public interface IDepartmentRepo
    {
        Task<List<Department>> GetCountriesAsync();
    }
}
