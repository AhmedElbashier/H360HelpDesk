using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdEscalationTimers
    {
        public int Id { get; set; }
        public int? Days { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
    }

}
