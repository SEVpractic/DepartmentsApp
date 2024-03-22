using DepartmentsApi.Models.Entities;
using DepartmentsApi.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace DepartmentsApi.Services
{
    public class DepartmentsStatusChecker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
		private readonly IMemoryCache memoryCache;

		public DepartmentsStatusChecker(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

        /// <summary>
        /// Метод периодического обновления инфомации о подразделениях
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                {
                    IDepartmentRepo departmentRepo = serviceScope.ServiceProvider.GetRequiredService<IDepartmentRepo>();
                    List<Department> departments = await departmentRepo.GetDepartmentsAsync();

					//ToDo вынести в конфиг
					memoryCache.Set("departments", departments, TimeSpan.FromSeconds(7));
				}
                
                //ToDo вынести в конфиг
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }
}
