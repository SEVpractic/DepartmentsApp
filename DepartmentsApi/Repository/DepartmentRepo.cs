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
            var departmentsForUpdate = await context.Departments
                .Where(el => departments.Select(dp => dp.DepartmentId).Contains(el.DepartmentId))
                .ToListAsync();

            foreach (Department departmentForUpdate in departmentsForUpdate)
            {
                Department department = departments.First(el => el.DepartmentId == departmentForUpdate.DepartmentId);

				if (department.ParentId != departmentForUpdate.ParentId) departmentForUpdate.ParentId = department.ParentId;
                if (!String.IsNullOrWhiteSpace(department.Name) && department.Name != departmentForUpdate.Name) departmentForUpdate.Name = department.Name;
				if (department.IsActive != departmentForUpdate.IsActive) departmentForUpdate.IsActive = department.IsActive;
            }

            return await context.SaveChangesAsync() > 0;
		}
	}
}
