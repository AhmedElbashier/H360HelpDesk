// EmailEscalationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Services
{
    public interface IEmailEscalationService
    {
        Task ProcessEscalationsAsync(CancellationToken cancellationToken);
    }

    public class EmailEscalationService : IEmailEscalationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailService _emailService;
        private readonly ILogger<EmailEscalationService> _logger;

        public EmailEscalationService(IServiceScopeFactory scopeFactory, EmailService emailService, ILogger<EmailEscalationService> logger)
        {
            _scopeFactory = scopeFactory;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task ProcessEscalationsAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.Now;
            var tickets = await dbContext.HdTickets
                .Where(t => t.DueDate <= now && (t.StatusID == 1 || t.StatusID == 2))
                .ToListAsync(cancellationToken);

            foreach (var ticket in tickets)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var mapping = dbContext.EscalationMappings
                    .Where(m => m.CategoryID == ticket.CategoryID
                             && m.DepartmentID == ticket.DepartmentID
                             && m.PriorityID == ticket.Priority
                             && (m.SubcategoryID == ticket.SubCategoryID || m.SubcategoryID == null))
                    .OrderByDescending(m => m.SubcategoryID != 0)
                    .FirstOrDefault();

                if (mapping == null)
                    continue;

                var elapsed = now - ticket.DueDate.Value;
                int? profileIdToNotify = null;

                if (elapsed >= mapping.Level3Delay && mapping.Level3ProfileID.HasValue)
                    profileIdToNotify = mapping.Level3ProfileID;
                else if (elapsed >= mapping.Level2Delay && mapping.Level2ProfileID.HasValue)
                    profileIdToNotify = mapping.Level2ProfileID;
                else if (elapsed >= mapping.Level1Delay)
                    profileIdToNotify = mapping.Level1ProfileID;

                if (profileIdToNotify == null)
                    continue;

                var profile = await dbContext.EscalationProfiles
                    .FirstOrDefaultAsync(p => p.ProfileID == profileIdToNotify, cancellationToken);

                if (profile == null)
                    continue;

                var ticketInfo = BuildTicketInformation(ticket, dbContext);
                await _emailService.SendTicketInformationEmailAsync(new List<TicketInformation> { ticketInfo }, profile.Email, ticket.DepartmentID);

                _logger.LogInformation($"[Escalation] Email sent to profile {profile.ProfileID} for ticket {ticket.Id}");
            }
        }

        private TicketInformation BuildTicketInformation(HdTickets ticket, AppDbContext db)
        {
            var department = db.HdDepartments.FirstOrDefault(d => d.DepartmentID == ticket.DepartmentID);
            var status = db.HdStatuses.FirstOrDefault(s => s.StatusID == ticket.StatusID);
            var priority = db.HdLevels.FirstOrDefault(p => p.LevelID == ticket.Priority);
            var agentUser = db.HdUsers.FirstOrDefault(u => u.User_Id == ticket.AssingedToUserID.ToString()) ?? new HdUsers { Firstname = "N", Lastname = "/A" };
            var backOfficeUser = db.HdUsers.FirstOrDefault(u => u.User_Id == ticket.AssingedToUserID.ToString()) ?? new HdUsers { Firstname = "N", Lastname = "/A" };

            return new TicketInformation
            {
                Id = ticket.Id,
                UserID = ticket.UserID.ToString(),
                AssingedToUserID = $"{agentUser.Firstname} {agentUser.Lastname}",
                AssingedToBackOfficeID = $"{backOfficeUser.Firstname} {backOfficeUser.Lastname}",
                DepartmentID = department?.Description,
                StatusID = status?.Description,
                PriorityID = priority?.Description,
                Subject = ticket.Subject,
                CustomerID = ticket.CustomerID.ToString(),
                CustomerName = ticket.CustomerName,
                Email = ticket.Email,
                Mobile = ticket.Mobile,
                StartDate = ticket.StartDate,
                DueDate = ticket.DueDate ?? DateTime.Now
            };
        }
    }
}
