using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdDepartments
    {
        [Key]
        public int DepartmentID { get; set; }

        public string Description { get; set; }

        public int CompanyID { get; set; }
    }
}
