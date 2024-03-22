using DepartmentsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Configs
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
