using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdLevels
    {
        [Key]
        public int LevelID { get; set; }

        public string Description { get; set; }

    }
}
