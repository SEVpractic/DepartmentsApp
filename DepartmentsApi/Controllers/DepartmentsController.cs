using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsApi.Controllers
{
    /// <summary>
    /// Контоллер работы с информацией о подразделениях
    /// </summary>
    [ApiController]
    [Route("/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService departmentsService;
        private readonly ILogger<DepartmentsController> logger;
        private readonly ResponseDto responseDto;

		public DepartmentsController(IDepartmentsService departmentsService, ILogger<DepartmentsController> logger)
        {
            this.departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.responseDto = new ResponseDto();
		}

        /// <summary>
        /// Возврат инфомации о подразделениях
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetAll()
        {
            try
            {
                List<DepartmentDto> departments = await departmentsService.GetDepartments();
                responseDto.Result = departments;
			}
            catch (Exception ex) 
            { 
                logger.LogWarning($"Ошибка метода GET /departments: {ex.Message}");
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.Message };
            }

            return responseDto;
        }

        /// <summary>
        /// Создание или обновление инфомации о подразделениях
        /// </summary>
        /// <param name="departmentDtos"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> CreateOrUpdate([FromBody] List<DepartmentDto> departmentDtos)
        {
            try
            {
				List<DepartmentDto> departments = await departmentsService.CreteOrUpdateAsync(departmentDtos);
				responseDto.Result = departments;
			}
			catch (Exception ex)
			{
                logger.LogWarning($"Ошибка метода POST /departments: {ex.Message}");
                responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { ex.Message };
			}

			return responseDto;
        }
    }
}
