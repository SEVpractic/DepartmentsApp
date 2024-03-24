using AutoMapper;
using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Models.Entities;
using DepartmentsApi.Repository;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace DepartmentsApi.Services
{
    /// <summary>
    /// Сервис работы с информацией о подразделениях.
    /// </summary>
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IMapper mapper;
		private readonly IMemoryCache memoryCache;
        private readonly ILogger<DepartmentsService> logger;

        public DepartmentsService(IDepartmentRepo departmentRepo, IMapper mapper, IMemoryCache memoryCache, ILogger<DepartmentsService> logger)
        {
            this.departmentRepo = departmentRepo ?? throw new ArgumentNullException(nameof(departmentRepo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Метод создания и обновления информации о подразделениях.
        /// </summary>
        /// <param name="departmentDtos"></param>
        /// <returns></returns>
		public async Task<List<DepartmentDto>> CreteOrUpdateAsync(List<DepartmentDto> departmentDtos)
		{
            List<Department> departments = await GetDepartmentsFromDb();

            // работа с коллекцией для обновления
            var departmentsForUpdate = departmentDtos.Where(el => el.DepartmentId > 0).ToList();
            if (departmentsForUpdate.Count > 0) await UpdateAsync(mapper.Map<List<Department>>(departmentsForUpdate));

            // работа с коллекцией для создания
			var departmentsForCreate = departmentDtos
                .Where(el => el.DepartmentId == null || el.DepartmentId <= 0)
                .Where(el => !departments.Select(dp => dp.Name).Contains(el.Name))
                .ToList();
            if (departmentsForCreate.Count > 0) await CreateAsync(mapper.Map<List<Department>>(departmentsForCreate));

            return await UpdateChachAndGet();
		}        

        /// <summary>
        /// Возврат информации о подразделениях из кэша, в случае отсутсвия принудительный запрос из БД.
        /// </summary>
        /// <returns></returns>
		public async Task<List<DepartmentDto>> GetDepartments()
        {
            if (memoryCache.TryGetValue("departments", out List<Department> departments))
            {
                Log.Information("Получена информации о подразделениях из кэша");
                return mapper.Map<List<DepartmentDto>>(departments);
            }            

            return await UpdateChachAndGet();
        }

        /// <summary>
        /// Обновлени информации о подразделениях в кэш
        /// </summary>
        /// <returns></returns>
        private async Task<List<DepartmentDto>> UpdateChachAndGet()
        {
            List<Department> departments = await GetDepartmentsFromDb();
            Log.Information("Получена информации о подразделениях из БД");
            memoryCache.Set("departments", departments, TimeSpan.FromSeconds(7));
			return mapper.Map<List<DepartmentDto>>(departments);
		}

        /// <summary>
        /// Запрос информации о подразделениях из БД
        /// </summary>
        /// <returns></returns>
        private async Task<List<Department>> GetDepartmentsFromDb()
        {
            return await departmentRepo.GetDepartmentsAsync();
        }

        /// <summary>
        /// Обновление коллекции подразделений
        /// </summary>
        /// <param name="departmentsForUpdate"></param>
        /// <returns></returns>
        private async Task<bool> UpdateAsync(List<Department> departmentsForUpdate)
        {
            bool isSuccess = await departmentRepo.UpdateDepartments(mapper.Map<List<Department>>(departmentsForUpdate));

            if (isSuccess)
            {
                Log.Information($"Обновлена информация по {departmentsForUpdate.Count} элементам");
            }
            else
            {
                Log.Warning("Обновление элементов не произведено");
            }

            return isSuccess;
        }

        /// <summary>
        /// Создание коллекции подразделений
        /// </summary>
        /// <param name="departmentsForCreate"></param>
        /// <returns></returns>
        private async Task<bool> CreateAsync(List<Department> departmentsForCreate)
        {
            bool isSuccess = await departmentRepo.CreateDepartments(mapper.Map<List<Department>>(departmentsForCreate));

            if (isSuccess)
            {
                Log.Information($"Добавлено {departmentsForCreate.Count} элементов");
            }
            else
            {
                Log.Warning("Добавление элементов не произведено");
            }

            return isSuccess;
        }
    }
}
