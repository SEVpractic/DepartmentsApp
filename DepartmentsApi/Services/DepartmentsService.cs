using AutoMapper;
using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Models.Entities;
using DepartmentsApi.Repository;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace DepartmentsApi.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IMapper mapper;
		private readonly IMemoryCache memoryCache;

		public DepartmentsService(IDepartmentRepo departmentRepo, IMapper mapper, IMemoryCache memoryCache)
        {
            this.departmentRepo = departmentRepo ?? throw new ArgumentNullException(nameof(departmentRepo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

		public async Task<List<DepartmentDto>> CreteOrUpdateAsync(List<DepartmentDto> departmentDtos)
		{
            var departmentsForUpdate = departmentDtos.Where(el => el.DepartmentId > 0).ToList();
            if (departmentsForUpdate.Count > 0)
            {
                await departmentRepo.UpdateDepartments(mapper.Map<List<Department>>(departmentsForUpdate));
            }

			var departmentsForCreate = departmentDtos.Where(el => el.DepartmentId == null || el.DepartmentId <= 0 ).ToList();
            if (departmentsForCreate.Count > 0)
            {
				await departmentRepo.CreateDepartments(mapper.Map<List<Department>>(departmentsForCreate));
			}

            return await GetDepartmentsFromDb();
		}

		public async Task<List<DepartmentDto>> GetDepartments()
        {
            var test = memoryCache.Get("departments");
            if (memoryCache.TryGetValue("departments", out List<Department> departments))
            {
                return mapper.Map<List<DepartmentDto>>(departments);
            }

            return await GetDepartmentsFromDb();
        }

        private async Task<List<DepartmentDto>> GetDepartmentsFromDb()
        {
			List<Department> departments = await departmentRepo.GetDepartmentsAsync();
			memoryCache.Set("departments", departments, TimeSpan.FromSeconds(7));
			return mapper.Map<List<DepartmentDto>>(departments);
		}
    }
}
