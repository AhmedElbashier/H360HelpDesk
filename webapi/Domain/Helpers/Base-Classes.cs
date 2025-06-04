using webapi.Domain.Models;

namespace webapi.Domain.Helpers
{
    public class ClientInfo
    {

        public string IpAddress { get; set; }


        public string Hostname { get; set; }

    }
    public class AddUserToGroupRequest
    {
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }
    public class UpdateLastSeenDto
    {
        public DateTime LastSeen { get; set; }
    }
    public class UpdateStatusDto
    {

        public string Status { get; set; }

    }
    public class UpdateLastLogoutDateDto
    {
        public DateTime LastLogoutDate { get; set; }
    }
    public class TicketWithFileUpload
    {

        public HdTickets Ticket { get; set; }


        public HdFileAttachments File { get; set; }

    }
    public class TicketTakeOverRequest
    {
        public int Id { get; set; }
        public int UserID { get; set; }
    }


    public class AgentReport
    {
        public int StatusID { get; set; }
        public int DepartmentID { get; set; }
        public int LevelID { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public DateTime startdate  { get; set; }
        public DateTime endDate { get; set; }
    }

    public class SupervisorReport
    {
        public int StatusID { get; set; }
        public int DepartmentID { get; set; }
        public int LevelID { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public int AssignedToUserID { get; set; }
        public DateTime startdate { get; set; }
        public DateTime endDate { get; set; }
    }

    public class SmsRequestModel
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }

    public class TicketInformation
    {
        public int EscalationID { get; set; }
        public int Id { get; set; }
        public string UserID { get; set; }
        public string AssingedToUserID { get; set; }
        public string Subject { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string AssingedToBackOfficeID { get; set; }
        public string DepartmentID { get; set; }
        public string PriorityID { get; set; }
        public string StatusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class ResetPasswordReq
    {
        public int UserID { get; set; }
        public string New_Password { get; set; }
    }

    public class SmsRequest
    {
        public string PhoneNumber { get; set; }
    }

    public class OpenTicketEmailTemplate
    {
        public int TicketID { get; set; }
        public string Category { get; set; }
        public string WorkingDays { get; set; }
    }
    public class CloseTicketEmailTemplate
    {
        public int TicketID { get; set; }
        public string Reply { get; set; }
    }

}
