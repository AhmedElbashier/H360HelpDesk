using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class EscalationLevel
    {
        [Key]
        public int LevelID { get; set; }
        public string LevelName { get; set; }

        public ICollection<EscalationProfile>? Profiles { get; set; } // <-- Make nullable
    }

}
