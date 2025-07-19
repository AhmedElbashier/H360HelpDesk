using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class EscalationMapping
    {
        [Key]
        public int MappingID { get; set; }

        public int DepartmentID { get; set; }
        public int CategoryID { get; set; }
        public int SubcategoryID { get; set; }
        public int PriorityID { get; set; }

        public int Level1ProfileID { get; set; }
        public int? Level2ProfileID { get; set; }
        public int? Level3ProfileID { get; set; }

        public TimeSpan? Level1Delay { get; set; } // e.g. 1h
        public TimeSpan? Level2Delay { get; set; } // e.g. 3h
        public TimeSpan? Level3Delay { get; set; } // e.g. 6h
    }

}
