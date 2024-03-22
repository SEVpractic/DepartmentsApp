using DepartmentsApi.Configs;
using DepartmentsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Repository
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly RepositoryContext context;
        public DepartmentRepo(RepositoryContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Department>> GetCountriesAsync()
        {
            return await context.Departments.OrderBy(el => el.Name).ToListAsync();
        }
    }
}
