using System.Net.Mail;
using System.Net;
using webapi.Domain.Helpers;
using webapi.Domain.Models;
using System.Net.Http;
using Org.BouncyCastle.Cms;
using OfficeOpenXml;
using Microsoft.AspNetCore.Html;
using jsreport.Local;
using jsreport.Types;
using jsreport.Binary;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Domain.Services
{
    public class EmailService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EmailService> _logger;


        public EmailService(AppDbContext context, IWebHostEnvironment environment, ILogger<EmailService> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }


        public async Task SendTicketInformationEmailAsync(List<TicketInformation> tickets, string escalationEmail, int escalationDepartment)
        {
            try
            {
                _logger.LogInformation("[Email] Preparing to send escalation email to {Email} for Department ID: {DepartmentID}", escalationEmail, escalationDepartment);

                if (string.IsNullOrWhiteSpace(escalationEmail))
                {
                    _logger.LogError("[Email] Escalation email is null or empty. Aborting send.");
                    return;
                }

                escalationEmail = escalationEmail.Trim();

                var smtpSettings = _context.SmtpSettings
                    .OrderByDescending(t => t.Id)
                    .FirstOrDefault();

                if (smtpSettings == null)
                {
                    _logger.LogError("[Email] SMTP settings not found in database.");
                    return;
                }

                var department = _context.HdDepartments
                    .FirstOrDefault(x => x.DepartmentID == escalationDepartment);

                var departmentName = department?.Description ?? $"ID {escalationDepartment}";

                using var client = new SmtpClient(smtpSettings.Host)
                {
                    Port = smtpSettings.Port,
                    Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
                    EnableSsl = smtpSettings.UseSsl
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.FromAddress, smtpSettings.DisplayName),
                    Subject = $"Tickets Escalation Report: {departmentName} Dept",
                    IsBodyHtml = true
                };

                try
                {
                    // 📎 Add recipient
                    mailMessage.To.Add(escalationEmail);

                    // 🖼️ Embed logo if it exists
                    string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "logo.png");
                    if (!File.Exists(logoPath))
                    {
                        _logger.LogWarning("[Email] Logo image not found at path: {Path}", logoPath);
                    }

                    string logoBase64 = File.Exists(logoPath) ? ImageToBase64(logoPath) : "";
                    string imgSrc = $"data:image/png;base64,{logoBase64}";

                    // 📧 Email body
                    _logger.LogDebug("[Email] Building email HTML body...");
                    string emailBody = BuildTicketEmailBody(tickets, departmentName);
                    mailMessage.Body = emailBody;

                    // 📎 Attachments
                    _logger.LogDebug("[Email] Creating attachments...");

                    byte[] excelBytes = GenerateExcelFromTickets(tickets).GetAsByteArray();
                    byte[] pdfBytes = ConvertHtmlToPdf(emailBody);

                    var excelStream = new MemoryStream(excelBytes);
                    var pdfStream = new MemoryStream(pdfBytes);

                    var excelAttachment = new Attachment(excelStream, "TicketInformation.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    var pdfAttachment = new Attachment(pdfStream, "TicketInformation.pdf", "application/pdf");

                    mailMessage.Attachments.Add(excelAttachment);
                    mailMessage.Attachments.Add(pdfAttachment);

                    _logger.LogInformation("[Email] Prepared email with {Count} tickets and 2 attachments.", tickets?.Count ?? 0);

                    // 📤 Send email
                    _logger.LogInformation("[Email] Sending email to: {Email}", escalationEmail);
                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation("[Email] Email sent successfully to: {Email}", escalationEmail);
                }
                catch (SmtpException smtpEx)
                {
                    _logger.LogError("[Email] SMTP error: {Message}", smtpEx.Message);

                    if (smtpEx.StatusCode != default)
                        _logger.LogError("[Email] SMTP Status Code: {Status}", smtpEx.StatusCode);

                    if (smtpEx.InnerException != null)
                        _logger.LogError("[Email] Inner Exception: {Inner}", smtpEx.InnerException.Message);

                    _logger.LogError("[Email] Stack Trace:\n{StackTrace}", smtpEx.StackTrace);
                }
                catch (Exception innerEx)
                {
                    _logger.LogError("[Email] Error while preparing/sending email: {Message}", innerEx.Message);
                    _logger.LogError(innerEx, "[Email] Full exception details");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Email] Unexpected error occurred during email escalation.");
                throw;
            }
        }


        public async Task SendOpenEmailAsync(string Email, int TicketID, string Category, string WorkingDays)
        {
            try
            {
                var emailTemplate = new OpenTicketEmailTemplate
                {
                    TicketID = TicketID,
                    Category = Category,
                    WorkingDays = WorkingDays,
                };
                var _smtpSettings = _context.SmtpSettings
                   .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                   .FirstOrDefault();
                using (var client = new SmtpClient(_smtpSettings.Host))
                {
                    client.Port = _smtpSettings.Port;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                        Subject = "Your Ticket No #" + TicketID + " Updates",
                        IsBodyHtml = true,
                    };
                    var emailBody = BuildOpenTicketEmailBody(emailTemplate);
                    mailMessage.Body = emailBody;
                    mailMessage.To.Add(Email);

                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (SmtpException smtpException)
            {
                Console.WriteLine($"SMTP error: {smtpException.Message}");

                if (smtpException.StatusCode != default(SmtpStatusCode))
                {
                    Console.WriteLine($"SMTP Status Code: {smtpException.StatusCode}");
                }

                if (smtpException.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {smtpException.InnerException.Message}");
                }

                Console.WriteLine($"Stack Trace: {smtpException.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task SendCloseEmailAsync(string Email, int TicketID, string Reply)
        {
            try
            {
                var emailTemplate = new CloseTicketEmailTemplate
                {
                    TicketID = TicketID,
                    Reply = "your issue has been resolved successfully",
                };
                var _smtpSettings = _context.SmtpSettings
                   .OrderByDescending(t => t.Id) // Assuming 'Id' is your auto-incrementing primary key
                   .FirstOrDefault();
                using (var client = new SmtpClient(_smtpSettings.Host))
                {
                    client.Port = _smtpSettings.Port;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                        Subject = "Your Ticket No #" + TicketID + " Updates",
                        IsBodyHtml = true,
                    };
                    var emailBody = BuildCloseTicketEmailBody(emailTemplate);
                    mailMessage.Body = emailBody;
                    mailMessage.To.Add(Email);

                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (SmtpException smtpException)
            {
                Console.WriteLine($"SMTP error: {smtpException.Message}");

                if (smtpException.StatusCode != default(SmtpStatusCode))
                {
                    Console.WriteLine($"SMTP Status Code: {smtpException.StatusCode}");
                }

                if (smtpException.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {smtpException.InnerException.Message}");
                }

                Console.WriteLine($"Stack Trace: {smtpException.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private HdEscalation GetEscalationByEmail(int escalationid)
        {

            return (HdEscalation)_context.HdEscalation.Where(escalation => escalation.EscalationID == (escalationid));
        }
        public string BuildOpenTicketEmailBody(OpenTicketEmailTemplate templateData)
        {
            string body = $@"
        <!DOCTYPE html>
        <html>
        <head>
          <title>Ticket Update</title>
        </head>
        <body style='font-family: Arial, sans-serif; margin:0; padding:0;'>
          <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
            <tr>
              <td style='padding: 20px 0 30px 0;'>
                <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse;'>
                  <tr>
                    <td align='center' style='padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;'>
                      Buruj Cooperative Insurance
                    </td>
                  </tr>
                </table>
                <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse;'>
                  <tr>
                    <td style='padding: 40px 30px 40px 30px; color: #153643; font-size: 16px; line-height: 24px;'>
                      <p>Dear Customer,</p>
                      <p>A ticket has been opened with the number {templateData.TicketID} to {templateData.Category}. You will be contacted within {templateData.WorkingDays} working days.</p>
                      <p>عميلنا العزيز، تم فتح تذكرة برقم {templateData.TicketID} لـ{templateData.Category} وسيتم التواصل معكم خلال {templateData.WorkingDays} أيام عمل.</p>
                      <p style='margin-top: 30px;'>Company Customer Care Department <br> شركة بروج للتامين التعاوني - ادارة العناية بالعملاء</p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>";
            return body;
        }


        public string BuildCloseTicketEmailBody(CloseTicketEmailTemplate templateData)
        {
            string body = $@"
        <!DOCTYPE html>
        <html>
        <head>
          <title>Ticket Update</title>
        </head>
        <body style='font-family: Arial, sans-serif; margin:0; padding:0;'>
          <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
            <tr>
              <td style='padding: 20px 0 30px 0;'>
                <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse;'>
                  <tr>
                    <td align='center' style='padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;'>
                      Buruj Cooperative Insurance
                    </td>
                  </tr>
                </table>
                <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse;'>
                  <tr>
                    <td style='padding: 40px 30px 40px 30px; color: #153643; font-size: 16px; line-height: 24px;'>
                      <p>Dear customer,</p>
                      <p>Your request with ticket number {templateData.TicketID} has been closed. We would like to inform you that {templateData.Reply}. For more information, you can contact us via our toll-free number 8001240201 or through email at customercare@burujinsurance.com</p>
                      <p>عزيزي العميل، لقد تم إغلاق طلبكم رقم {templateData.TicketID}. ونود افادتكم بان {templateData.Reply} وللمزيد من المعلومات يمكنكم التواصل معنا عبر الهاتف المجاني 8001240201 او عبر البريد الالكتروني customercare@burujinsurance.com</p>
                      <p style='margin-top: 30px;'>Company Customer Care Department <br> شركة بروج للتامين التعاوني - ادارة العناية بالعملاء</p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>";
            return body;
        }

        private string BuildTicketEmailBody(List<TicketInformation> tickets, string departmentName)
        {
            // Build the email body content for all tickets
            var body = @"
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }
        th, td {
            border: 1px solid #dddddd;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
        tr:hover {
            background-color: #f5f5f5;
        }
        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .logo {
            width: 150px;
        }
        .footer {
            margin-top: 20px;
            font-size: 0.8em;
            color: #888;
        }
    </style>
</head>
<body>";
            body += $@"

<div class='header'>
    <h2>Tickets Escalation Report: {departmentName}</h2>
</div>
<table>
    <tr>
        <th>Ticket ID</th>
        <th>Department</th>
        <th>Agent Name</th>
        <th>BackOffice User</th>
        <th>Status</th>
        <th>Priority</th>
        <th>Subject</th>
        <th>CustomerID</th>
        <th>Customer Name</th>
        <th>Customer Email</th>
        <th>Customer Phone</th>
        <th>Start Date</th>
        <th>Due Date</th>
    </tr>";

            foreach (var ticket in tickets)
            {
                body += $@"
    <tr>
        <td>{ticket.Id}</td>
        <td>{ticket.DepartmentID}</td>
        <td>{ticket.AssingedToUserID}</td>
        <td>{ticket.AssingedToBackOfficeID}</td>
        <td>{ticket.StatusID}</td>
        <td>{ticket.PriorityID}</td>
        <td>{ticket.Subject}</td>
        <td>{ticket.CustomerID}</td>
        <td>{ticket.CustomerName}</td>
        <td>{ticket.Email}</td>
        <td>{ticket.Mobile}</td>
        <td>{ticket.StartDate}</td>
        <td>{ticket.DueDate}</td>
    </tr>";
            }

            body += @"
</table>
<div class='footer'>
    © " + DateTime.Now.Year + @" VOCALCOM MEA - All rights reserved.
</div>
</body>
</html>";

            return body;
        }

        private string BuildTicketEmailBody2(List<TicketInformation> tickets, string departmentName)
        {
            var t = tickets.FirstOrDefault();
            if (t == null) return "<html><body>No content</body></html>";

            return $@"
<html>
<head>
  <style>
    body {{ font-family: Arial, sans-serif; }}
    .card {{ border:1px solid #eee; padding:16px; border-radius:8px; }}
    .row span {{ display:block; margin:2px 0; }}
  </style>
</head>
<body>
  <h3>New Ticket Created — {departmentName}</h3>
  <div class='card'>
    <div class='row'><span><b>ID:</b> {t.Id}</span></div>
    <div class='row'><span><b>Subject:</b> {System.Net.WebUtility.HtmlEncode(t.Subject)}</span></div>
    <div class='row'><span><b>Status:</b> {t.StatusID}</span></div>
    <div class='row'><span><b>Priority:</b> {t.PriorityID}</span></div>
    <div class='row'><span><b>Customer:</b> {System.Net.WebUtility.HtmlEncode(t.CustomerName)} ({t.CustomerID})</span></div>
    <div class='row'><span><b>Email:</b> {t.Email}</span></div>
    <div class='row'><span><b>Phone:</b> {t.Mobile}</span></div>
    <div class='row'><span><b>Start:</b> {t.StartDate:yyyy-MM-dd HH:mm}</span></div>
    <div class='row'><span><b>Due:</b> {t.DueDate:yyyy-MM-dd HH:mm}</span></div>
  </div>
  <p style='color:#888;font-size:12px;margin-top:12px;'>© {DateTime.Now.Year} VOCALCOM MEA</p>
</body>
</html>";
        }


        public string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);
            return base64Image;
        }
        private ExcelPackage GenerateExcelFromTickets(List<TicketInformation> tickets)
        {
            var excelPackage = new ExcelPackage();
            var workSheet = excelPackage.Workbook.Worksheets.Add("Tickets");
            workSheet.Cells["A1"].LoadFromCollection(tickets, true); // true means it will generate headers from property names
            return excelPackage;
        }

        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            var rs = new LocalReporting()
                .UseBinary(jsreport.Binary.JsReportBinary.GetBinary())
                .AsUtility()
                .Create();

            var report = rs.RenderAsync(new RenderRequest
            {
                Template = new Template
                {
                    Content = htmlContent,
                    Engine = Engine.None,
                    Recipe = Recipe.ChromePdf
                }
            }).Result;

            using (MemoryStream ms = new MemoryStream())
            {
                report.Content.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public async Task SendBackOfficeNewTicketAsync(
    IEnumerable<string> recipients,
    TicketInformation info,
    int departmentId)
        {
            var departmentName = _context.HdDepartments
                .FirstOrDefault(x => x.DepartmentID == departmentId)?.Description ?? $"ID {departmentId}";

            var html = BuildTicketEmailBody2(new List<TicketInformation> { info }, departmentName);
            var smtp = _context.SmtpSettings.OrderByDescending(t => t.Id).FirstOrDefault();
            if (smtp == null || recipients == null) return;

            foreach (var email in recipients.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(email)) continue;

                using var client = new SmtpClient(smtp.Host)
                {
                    Port = smtp.Port,
                    Credentials = new NetworkCredential(smtp.Username, smtp.Password),
                    EnableSsl = smtp.UseSsl
                };

                using var msg = new MailMessage
                {
                    From = new MailAddress(smtp.FromAddress, smtp.DisplayName),
                    Subject = $"[New Ticket #{info.Id}] {info.Subject}",
                    Body = html,
                    IsBodyHtml = true
                };
                msg.To.Add(email.Trim());
                await client.SendMailAsync(msg);
            }
        }
    }
}
