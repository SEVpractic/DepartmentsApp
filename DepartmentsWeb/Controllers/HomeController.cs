using DepartmentsWeb.Models;
using DepartmentsWeb.Models.Dto;
using DepartmentsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DepartmentsWeb.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IDepartmentsService departmentsService;

		public HomeController(ILogger<HomeController> logger, IDepartmentsService departmentsService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
		}

		public async Task<IActionResult> Index(string? searchString)
		{
			DepartmentsListDto departmentsListDto = new DepartmentsListDto()
			{
				Departments = await departmentsService.GetFromApiAsync()
			};

			if(!String.IsNullOrEmpty(searchString))
			{
				departmentsListDto.Departments = departmentsListDto.Departments
                    .Where(el => Regex.IsMatch(
						el.Name, 
						Regex.Escape(searchString),
						RegexOptions.IgnoreCase)
					)
					.ToList();
			}

            return View(departmentsListDto);
		}

		public async Task<IActionResult> Departments(string? searchString)
        {
            List<DepartmentDto> departments = await departmentsService.GetFromApiAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments
                    .Where(el => Regex.IsMatch(
                        el.Name,
                        Regex.Escape(searchString),
                        RegexOptions.IgnoreCase)
                    )
                    .ToList();
            }

            return View(departments);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> Synchronize()
		{
            if (Request.Form.Files.Count > 0 
				&& Request.Form.Files[0] != null 
				&& Request.Form.Files[0].ContentType == "text/plain")
            {
                var file = Request.Form.Files[0];
				await departmentsService.SynchronizeDb(file.OpenReadStream());
            }

            return RedirectToAction("Index", "Home");
		}

    }
}