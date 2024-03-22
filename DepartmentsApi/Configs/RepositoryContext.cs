using DepartmentsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Configs
{
    public class RepositoryContext : DbContext
    {
        //Todo
        //drop table if exists "Departments", "__EFMigrationsHistory"
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
                Name = "Main Department",
                IsActive = true
            },
                new Department
            {
                DepartmentId = 2,
                Name = "Department11",
                IsActive = true,
                ParentId = 1
            },
                new Department
            {
                DepartmentId = 3,
                Name = "Department12",
                IsActive = true,
                ParentId = 1
            },
                new Department
            {
                DepartmentId = 4,
                Name = "Department21",
                IsActive = true,
                ParentId = 2
            },
                new Department
            {
                DepartmentId = 5,
                Name = "Department22",
                IsActive = true,
                ParentId = 2
            });
        }
    }
}
