using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdFileAttachments
    {
        [Key]
        public int FileID { get; set; }
        public int TicketID { get; set; }
        public int? CommentID { get; set; }
        public int UserID { get; set; }

        public string FileName { get; set; }


        public string FileHash { get; set; }


        public string FileData { get; set; }

        public long FileSize { get; set; }
    }
}
