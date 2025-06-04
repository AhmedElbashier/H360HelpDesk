using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdEscalation
    {
        [Key]
        public int EscalationID { get; set; }
        public int DepartmentID { get; set; }
        public int levelID { get; set; }
        public int Days { get; set; }

        public string Emails { get; set; }

    }
}
