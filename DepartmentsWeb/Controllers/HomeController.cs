using DepartmentsWeb.Models;
using DepartmentsWeb.Models.Dto;
using DepartmentsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DepartmentsWeb.Controllers
{
    /// <summary>
    /// Контоллер работы с информацией о подразделениях
    /// </summary>
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> logger;
		private readonly IDepartmentsService departmentsService;

		public HomeController(ILogger<HomeController> logger, IDepartmentsService departmentsService)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
		}

        /// <summary>
        /// Отображение инфомации о подразделениях в виде дерева
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string? searchString)
		{
			DepartmentsListDto departmentsListDto = new DepartmentsListDto()
			{
				Departments = await departmentsService.GetFromApiAsync()
			};

			if(!String.IsNullOrEmpty(searchString))
			{
                var searchingEl = departmentsListDto.Departments
                    .FirstOrDefault(el => Regex.IsMatch(
                        el.Name,
                        Regex.Escape(searchString),
                        RegexOptions.IgnoreCase)
                    );

                departmentsListDto.Seed = searchingEl != null ? searchingEl.ParentId : null;

                departmentsListDto.Departments.RemoveAll(el => el.ParentId == searchingEl.ParentId && el.DepartmentId != searchingEl.DepartmentId);             
            }

            return View(departmentsListDto);
		}

        /// <summary>
        /// Отображение инфомации о подразделениях в виде списка
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Создание или обновление инфомации о подразделениях
        /// </summary>
        /// <returns></returns>
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