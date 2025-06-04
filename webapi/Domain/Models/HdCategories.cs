using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdCategories
    {
        [Key]
        public int CategoryID { get; set; }

        public string Description { get; set; }

        public int DepartmentID { get; set; }

    }
}
