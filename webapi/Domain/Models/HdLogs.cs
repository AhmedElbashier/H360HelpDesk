using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdLogs
    {
        [Key]
        public int LogID { get; set; }
        public int UserID { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
