using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdComments
    {
        [Key]
        public int CommentID { get; set; }
        public int TicketID { get; set; }
        public DateTime CommentDate { get; set; }
        public int UserID { get; set; }

        public string Body { get; set; }

        public bool TicketFlag { get; set; }
    }

    public class HdCommentsReq
    {
        [Key]
        public int TicketID { get; set; }
        public int UserID { get; set; }
        public string Body { get; set; }

    }
}
