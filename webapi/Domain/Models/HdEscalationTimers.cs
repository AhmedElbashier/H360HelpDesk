using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdEscalationTimers
    {
        [Key]
        public int TimerID { get; set; }

        public int Hours { get; set; }

    }
}
