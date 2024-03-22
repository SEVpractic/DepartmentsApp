using AutoMapper;
using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Repository;
using Microsoft.Extensions.Caching.Memory;

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

        public List<DepartmentDto> GetDepartmentsAsync()
        {
            var test = memoryCache.Get("departments");
            if (memoryCache.TryGetValue("departments", out var departments))
            {
                return mapper.Map<List<DepartmentDto>>(departments);
            }

            return new List<DepartmentDto>();
        }
    }
}
