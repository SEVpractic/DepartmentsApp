using DepartmentsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Configs
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Department> Departments { get; set; } = null!;

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    DepartmentId = 1,
                    Name = "Главное Управление",
                    IsActive = true
                },
                new Department
                {
                    DepartmentId = 2,
                    Name = "Депортамент времени",
                    IsActive = true,
                    ParentId = 1
                },
                new Department
                {
                    DepartmentId = 3,
                    Name = "Подразделение искажения",
                    IsActive = false,
                    ParentId = 1
                },
                new Department
                {
                    DepartmentId = 4,
                    Name = "Управление правды",
                    IsActive = true,
                    ParentId = 2
                },
                new Department
                {
                    DepartmentId = 5,
                    Name = "Бюро наблюдений",
                    IsActive = true,
                    ParentId = 2
                });
        }
    }
}
