using AutoMapper;
using DepartmentsApi.Models.Dtos;
using DepartmentsApi.Models.Entities;

namespace DepartmentsApi.Configs
{
    public class MappingConfig
    {
        public static MapperConfiguration GetMapperConfiguraton()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Department, DepartmentDto>().ReverseMap();
            });
        }
    }
}
