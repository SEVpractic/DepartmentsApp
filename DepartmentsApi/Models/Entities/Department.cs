using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace DepartmentsApi.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public long DepartmentId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Parent { get; set; }
    }
}
