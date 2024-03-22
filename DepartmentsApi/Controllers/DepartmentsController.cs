using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Models.Entities;
using DepartmentsApi.Repository;
using DepartmentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApi.Controllers
{
    [ApiController]
    [Route("/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService departmentsService;

        public DepartmentsController(IDepartmentsService departmentsService)
        {
            this.departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
        }

        [HttpGet]
        public async Task<List<DepartmentDto>> GetDepartments()
        {

            return await departmentsService.GetDepartmentsAsync();
        }
    }
}
