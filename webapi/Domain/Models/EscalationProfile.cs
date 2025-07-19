using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class EscalationProfile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Title { get; set; }

        public int EscalationLevelID { get; set; }
        
    }
}
