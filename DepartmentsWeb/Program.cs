using DepartmentsWeb.Configs;
using Serilog;

namespace DepartmentsWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
            Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.WriteTo.File("logs/DepartmentsWebLogs.txt", rollingInterval: RollingInterval.Month)
				.CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog();
            builder.Services.AddHttpClient();

			builder.Services.GetServicesConfig();

			builder.Services.AddControllersWithViews();
			
			

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}