using DepartmentsWeb.Services;

namespace DepartmentsWeb.Configs
{
	public static class ServicesConfig
	{
		/// <summary> Получение конфигурации сервисов </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection GetServicesConfig(this IServiceCollection services)
		{
			services.AddScoped<IDepartmentsService, DepartmentsService>();

			return services;
		}
	}
}
