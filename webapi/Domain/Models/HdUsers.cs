using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdUsers
    {
        [Key]
        public int Id { get; set; }

        public string User_Id { get; set; }


        public string Username { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }


        public string Firstname { get; set; }


        public string Lastname { get; set; }


        public string Phone { get; set; }


        public string IPAddress { get; set; }


        public string HostName { get; set; }

        public string? Department_Id { get; set; }
        public string? Department_Name { get; set; }

        public string Status { get; set; }

        public bool IsAdministrator { get; set; }
        public bool IsSuperVisor { get; set; }
        public bool IsDepartmentRestrictedSupervisor { get; set; }

        public bool IsAgent { get; set; }
        public bool IsBackOffice { get; set; }
        public bool Disabled { get; set; }
        public bool DarkMode { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public DateTime LastLogoutDate { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public bool Deleted { get; set; }



    }
}
