using AutoMapper;
using DepartmentsApi.Repository;
using DepartmentsApi.Services;
using Microsoft.EntityFrameworkCore;

namespace DepartmentsApi.Configs
{
    public static class ServicesConfig
    {
        /// <summary> Получение конфигурации сервисов </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection GetServicesConfig(this IServiceCollection services)
        {
            IMapper mapper = MappingConfig
                .GetMapperConfiguraton()
                .CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHostedService<DepartmentsStatusChecker>();

            services.AddScoped<IDepartmentRepo, DepartmentRepo>();

            services.AddScoped<IDepartmentsService, DepartmentsService>();

            return services;
        }

        /// <summary> Получение конфигурации репозитория </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection GetReposConfig(this IServiceCollection services, string DBConnectionString)
        {
            services.AddDbContext<RepositoryContext>(
                    DbContextOptions => {
                        DbContextOptions.UseNpgsql(DBConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                    });

            return services;
        }        
    }
}
