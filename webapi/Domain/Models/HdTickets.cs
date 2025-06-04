using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdTickets
    {
        [Key]
        public int Id { get; set; }
        public int? CustomerID { get; set; }
        public int Indice { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int DepartmentID { get; set; }
        public int ChannelID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? AssingedToBackOfficeID { get; set; }
        public int? AssingedToUserID { get; set; }

        public string Subject { get; set; }


        public string Body { get; set; }

        public int StatusID { get; set; }
        public int RequestID { get; set; }
        public int Priority { get; set; }
        public int EscalationLevel { get; set; }
        public int UpdateByUser { get; set; }
        public DateTime? DueDate { get; set; }
        public bool SMSAlert { get; set; }
        public bool EmailAlert { get; set; }
        public bool Flag { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }


        public string CustomerName { get; set; }

        public int CompanyID { get; set; }
        public string DepartmentReply { get; set; }

    }
}
