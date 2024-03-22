using DepartmentsApi.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DepartmentsApi.Models.Dtos
{
    public class DepartmentDto
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
    }
}
