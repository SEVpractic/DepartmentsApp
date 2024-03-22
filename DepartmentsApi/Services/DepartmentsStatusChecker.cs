using DepartmentsApi.Repository;

namespace DepartmentsApi.Services
{
    public class DepartmentsStatusChecker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public DepartmentsStatusChecker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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
                    var test = await departmentRepo.GetDepartmentsAsync();
                }
                
                //ToDo вынести в конфиг
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }
}
