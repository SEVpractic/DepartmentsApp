using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Configs
{
    public static class ServicesConfig
    {
        // TODO вынести строку подключения в конфиг
        public static string DBConnectionString = "Host=localhost;Port=5432;Database=DepartmentsBD;Username=Wisebrain;Password=iamroot";

        /// <summary> Получение конфигурации репозитория </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection GetReposConfig(this IServiceCollection services)
        {
            services.AddDbContext<RepositoryContext>(
                    DbContextOptions => {
                        DbContextOptions.UseNpgsql(DBConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                    });

            return services;
        }
    }
}
