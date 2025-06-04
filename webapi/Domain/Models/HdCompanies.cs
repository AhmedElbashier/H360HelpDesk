using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdCompanies
    {
        [Key]
        public int CompanyID { get; set; }

        public string Description { get; set; }

    }
}
