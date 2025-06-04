using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdRequests
    {
        [Key]
        public int RequestID { get; set; }

        public string Description { get; set; }

        public int DepartmentID { get; set; }
    }
}
