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

		public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await context.Departments
                .OrderBy(el => el.DepartmentId)
                .ToListAsync();
        }

		public async Task<bool> CreateDepartments(List<Department> departments)
		{
			await context.Departments.AddRangeAsync(departments);
			return await context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateDepartments(List<Department> departments)
		{
            var ids = departments.Select(el => el.DepartmentId);
            var departmentsForUpdate = await context.Departments.Where(el => ids.Contains(el.DepartmentId)).ToListAsync();

            foreach (var department in departmentsForUpdate)
            {
                department.ParentId = departments.FirstOrDefault(el => el.DepartmentId == department.DepartmentId).ParentId;
            }
            return await context.SaveChangesAsync() > 0;
		}
	}
}
