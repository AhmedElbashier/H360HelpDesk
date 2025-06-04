using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdSubCategories
    {
        [Key]
        public int SubCategoryID { get; set; }

        public string Description { get; set; }

        public int CategoryID { get; set; }
    }
}
