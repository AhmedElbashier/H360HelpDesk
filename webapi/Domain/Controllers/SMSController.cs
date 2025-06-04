using System;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using webapi.Domain.Helpers;
using webapi.Domain.Models;

namespace webapi.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SMSController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("send-test-sms")]
        public async Task<IActionResult> SendSmsAsync([FromBody] SmsRequest smsRequest)
        {
            try
            {
                // Construct the URL with the phone number from the request body
                string baseurl = $"https://mshastra.com/sendurl.aspx?user=CustomerCA&pwd=_bu19jh6&senderid=BURUJ&mobileno={smsRequest.PhoneNumber}&msgtext=Hello&CountryCode=ALL";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(baseurl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Failed to send SMS. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the HTTP request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("send-open-notification-sms")]
        public async Task<IActionResult> SendOpenSmsAsync([FromBody] SmsRequest smsRequest,int TicketID, string Category, string WorkingDays)
        {
            try
            {
                var Body1 = "Dear Custromer, a ticket has been opened with the number "+TicketID+" to "+Category+". You will be contacted within "+ WorkingDays + " working days. \n";
                var Body2 = "عميلنا العزيز، تم فتح تذكرة برقم "+TicketID+" لـ" +Category+" وسيتم التواصل معكم خلال "+WorkingDays+ " أيام عمل.\n";
                var Body3 = "Buruj Cooperative Insurance - Company Customer Care Department \n شركة بروج للتامين التعاوني - ادارة العناية بالعملاء";
                var Body = Body1 + Body2 + Body3;
                // Construct the URL with the phone number from the request body
                string baseurl = $"https://mshastra.com/sendurl.aspx?user=CustomerCA&pwd=_bu19jh6&senderid=BURUJ&mobileno={smsRequest.PhoneNumber}&msgtext={Body}&CountryCode=ALL";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(baseurl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        DateTime commentDate = DateTime.Now;
                        var Comment = new HdComments
                        {
                            TicketID = TicketID,
                            CommentDate = commentDate,
                            UserID = 0,
                            Body = "SMS notification Sent to the Client (Ticket opened)",
                            TicketFlag = true
                        };
                        _context.HdComments.Add(Comment);
                        this._context.SaveChanges();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Failed to send SMS. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the HTTP request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("send-close-notification-sms")]
        public async Task<IActionResult> SendCloseSmsAsync([FromBody] SmsRequest smsRequest, int TicketID, string Reply)
        {
            try
            {
                var Body1 = "Dear customer , your request with ticket number "+TicketID+" has been closed. We would like to inform you that " + Reply + " . For more information, you can contact us via our toll-free number 8001240201 or through email at customercare@burujinsurance.com\n";
                var Body2 = "عزيزي العميل   لقد تم إغلاق طلبكم رقم " + TicketID + ". ونود افادتكم بان " + Reply + " وللمزيد من المعلومات يمكنكم  التواصل معنا عبر الهاتف المجاني 8001240201 او عبر البريد الالكتروني customercare@burujinsurance.com\n";
                var Body3 = "Buruj Cooperative Insurance - Company Customer Care Department \n شركة بروج للتامين التعاوني - ادارة العناية بالعملاء";
                var Body = Body1 + Body2 + Body3;
                // Construct the URL with the phone number from the request body
                string baseurl = $"https://mshastra.com/sendurl.aspx?user=CustomerCA&pwd=_bu19jh6&senderid=BURUJ&mobileno={smsRequest.PhoneNumber}&msgtext={Body}&CountryCode=ALL";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(baseurl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        DateTime commentDate = DateTime.Now;
                        var Comment  =  new HdComments
                        {
                        TicketID = TicketID,
                        CommentDate = commentDate,
                        UserID = 0,
                        Body = "SMS notification Sent to the Client (Ticket closed)",
                        TicketFlag = true
                    };
                        _context.HdComments.Add(Comment);
                        this._context.SaveChanges();
                        return Ok(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Failed to send SMS. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the HTTP request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
