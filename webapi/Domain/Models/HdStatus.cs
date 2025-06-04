using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdStatus
    {
        [Key]
        public int StatusID { get; set; }

        public string Description { get; set; }

    }
}
