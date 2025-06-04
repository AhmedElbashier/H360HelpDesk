using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Services
{
    public interface IEmailEscalationService
    {
        void ProcessEscalations();
    }
    public class EmailEscalationService : IEmailEscalationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailService _emailService;

        public EmailEscalationService(IServiceScopeFactory scopeFactory, EmailService emailService)
        {
            _scopeFactory = scopeFactory;
            _emailService = emailService;
        }

        public void ProcessEscalations()
        {
            // Get tickets with SLA date passed and StatusID 1 or 2
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var today = DateTime.Today;
                var HdEscalations = dbContext.HdEscalation;
                var groupedTickets = dbContext.HdTickets
                    .Where(ticket => ticket.DueDate <= today && (ticket.StatusID == 1 || ticket.StatusID == 2))
                    .GroupBy(t => t.EscalationLevel)
                    .ToList();

                foreach (var ticketGroup in groupedTickets)
                {
                    var ticketInformationList = new List<TicketInformation>();

                    foreach (var ticket in ticketGroup)
                    {
                        HdDepartments department = dbContext.HdDepartments
                            .Where(x => x.DepartmentID == ticket.DepartmentID)
                            .FirstOrDefault();
                        HdStatus status = dbContext.HdStatuses
                        .Where(x => x.StatusID == ticket.StatusID)
                        .FirstOrDefault();
                         HdLevels priority = dbContext.HdLevels
                        .Where(x => x.LevelID == ticket.Priority).FirstOrDefault();
                        HdUsers agentUser = dbContext.HdUsers
                        .Where(x => x.User_Id == ticket.AssingedToUserID.ToString()).FirstOrDefault();
                        HdUsers backOfficeUser = dbContext.HdUsers
                        .Where(x => x.User_Id == ticket.AssingedToUserID.ToString()).FirstOrDefault();
                        if (backOfficeUser == null)
                        {
                            backOfficeUser = new HdUsers
                            {
                                Firstname = "N",
                                Lastname = "/A"
                            };
                        }

                        if (agentUser == null)
                        {
                            agentUser = new HdUsers
                            {
                                Firstname = "N",
                                Lastname = "/A"
                            };
                        }
                        var ticketInfo = new TicketInformation
                        {
                            Id = ticket.Id,
                            UserID = ticket.UserID.ToString(),
                            AssingedToUserID = agentUser.Firstname + " " + agentUser.Lastname,
                            AssingedToBackOfficeID = backOfficeUser.Firstname + " " + backOfficeUser.Lastname,
                            DepartmentID = department.Description,
                            StatusID = status.Description,
                            PriorityID = priority.Description,
                            Subject = ticket.Subject,
                            CustomerID = ticket.CustomerID.ToString(),
                            CustomerName = ticket.CustomerName,
                            Email = ticket.Email,
                            Mobile = ticket.Mobile,
                            StartDate = ticket.StartDate,
                            DueDate = ticket.DueDate.Value,
                        };

                        ticketInformationList.Add(ticketInfo);
                    }

                    // Fetch the escalation record based on the group's escalation ID
                    var escalation = GetEscalationByEmail(ticketGroup.Key);
                    if (escalation == null) continue; // If no matching escalation found, skip this group
                                                      // Now call your email sending function, modified to send to the escalation email
                    _emailService.SendTicketInformationEmailAsync(ticketInformationList, escalation.Emails, escalation.DepartmentID);
                }

                //_emailService.SendTicketInformationEmailAsync(ticketInformationList);
            }
        }
        private HdEscalation GetEscalationByEmail(int escalationId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return dbContext.HdEscalation.FirstOrDefault(escalation => escalation.EscalationID == escalationId);
            }
        }


    }
}