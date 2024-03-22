using AutoMapper;
using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Repository;

namespace DepartmentsApi.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IMapper mapper;

        public DepartmentsService(IDepartmentRepo departmentRepo, IMapper mapper)
        {
            this.departmentRepo = departmentRepo ?? throw new ArgumentNullException(nameof(departmentRepo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            return mapper.Map<List<DepartmentDto>>(await departmentRepo.GetDepartmentsAsync());
        }
    }
}
