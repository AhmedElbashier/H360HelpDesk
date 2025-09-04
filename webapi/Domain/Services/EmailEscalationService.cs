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
            _logger.LogInformation("[Escalation] Fetching tickets due before {Now}", now);

            var tickets = await dbContext.HdTickets
                .Where(t => t.DueDate <= now && (t.StatusID == 1 || t.StatusID == 2))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("[Escalation] {Count} tickets found for escalation", tickets.Count);

            foreach (var ticket in tickets)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogDebug("[Escalation] Processing Ticket ID: {TicketId}", ticket.Id);

                if (ticket.EscalationLevel == null || ticket.EscalationLevel <= 0)
                {
                    _logger.LogWarning("[Escalation] Ticket ID {TicketId} has no EscalationLevel set", ticket.Id);
                    continue;
                }

                var level = await dbContext.EscalationLevels
                    .Include(l => l.Profiles)
                    .FirstOrDefaultAsync(l => l.LevelID == ticket.EscalationLevel, cancellationToken);

                if (level == null || level.Profiles == null || !level.Profiles.Any())
                {
                    _logger.LogWarning("[Escalation] No valid profiles found for EscalationLevel {LevelID} on Ticket ID {TicketId}", ticket.EscalationLevel, ticket.Id);
                    continue;
                }

                var ticketInfo = BuildTicketInformation(ticket, dbContext);

                foreach (var profile in level.Profiles.Where(p => !string.IsNullOrWhiteSpace(p.Email)))
                {
                    try
                    {
                        _logger.LogInformation("[Escalation] Sending escalation email to {Email} for Ticket ID {TicketId}", profile.Email, ticket.Id);
                        await _emailService.SendTicketInformationEmailAsync(
                            new List<TicketInformation> { ticketInfo },
                            profile.Email,
                            ticket.DepartmentID);

                        _logger.LogInformation("[Escalation] Email sent successfully to {Email} for Ticket ID {TicketId}", profile.Email, ticket.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[Escalation] Failed to send email to {Email} for Ticket ID {TicketId}", profile.Email, ticket.Id);
                    }
                }
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
