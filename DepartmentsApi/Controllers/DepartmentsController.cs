using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApi.Controllers
{
    [ApiController]
    [Route("/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService departmentsService;
		private readonly ResponseDto responseDto;

		public DepartmentsController(IDepartmentsService departmentsService)
        {
            this.departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
			this.responseDto = new ResponseDto();
		}

        [HttpGet]
        public async Task<object> GetAll()
        {
            try
            {
                List<DepartmentDto> departments = await departmentsService.GetDepartments(); 
                departmentsService.GetDepartments();
                responseDto.Result = departments;
			}
            catch (Exception ex) 
            { 
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.Message };
            }

            return responseDto;
        }

        [HttpPost]
        public async Task<object> CreateOrUpdate([FromBody] List<DepartmentDto> departmentDtos)
        {
            try
            {
				List<DepartmentDto> departments = await departmentsService.CreteOrUpdateAsync(departmentDtos);
				departmentsService.GetDepartments();
				responseDto.Result = departments;
			}
			catch (Exception ex)
			{
				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { ex.Message };
			}

			return responseDto;
        }
    }
}
