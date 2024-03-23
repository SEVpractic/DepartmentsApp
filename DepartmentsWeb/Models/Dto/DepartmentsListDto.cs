namespace DepartmentsWeb.Models.Dto
{
    public class DepartmentsListDto
    {
        public long? Seed { get; set; }
        public List<DepartmentDto> Departments { get; set; }
    }
}
