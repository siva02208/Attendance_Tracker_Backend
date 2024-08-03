using AttendenceTracker.Helper;
using AttendenceTracker.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;
using System.Diagnostics;


namespace AttendenceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IEmailService emailService;
        public CustomerController(IEmailService service)
        {
            this.emailService = service;
        }


        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] AttendenceData requestData)
        {
            try
            {
                MailRequest mailrequest = new MailRequest();
                mailrequest.ToEmail = requestData.Student.Email; 
                mailrequest.Subject = $"Absence Notification for {requestData.Student.Name} - ({requestData.Student.Branch})";
                mailrequest.Body = GetHtmlContent(requestData);
                await emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetHtmlContent(AttendenceData requestData)
        {
            string htmlContent = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Absence Notification</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .letter {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
    </style>
</head>
<body>
    <div class=""letter"">
        <h1>Absence Notification</h1>
        <p>Date: <b>{requestData.OnDate}</b></p>
        <p>Dear <b>{requestData.Student.Name}</b>,</p>
        <p>I hope this message finds you well. We wanted to inform you that you were marked as absent for college on <b>{requestData.OnDate}</b>, as per our records.</p>
        <p>You, a <b>{requestData.Student.Branch}</b> major, know the importance of attendance for academic progress and success. Regular attendance ensures you receive the full benefit of your educational experience in the {requestData.Student.Branch} program.</p>
        <p>For any future notices, information, or assistance related to your coursework and attendance, please feel free to reach out to your instructor,<b> {requestData.Teacher.StaffName}</b>, at <b>{requestData.Teacher.Email}</b></p>
        <p>If there was a specific reason for your absence on that day, please feel free to communicate with us at your earliest convenience. We understand that unforeseen circumstances may arise, and we are here to assist you.</p>
        <p>Please do not hesitate to contact <b>{requestData.Teacher.StaffName}</b> or the College Management and Attendance Monitoring Team for any assistance or inquiries regarding your attendance.</p>
        <p>Thank you for your attention to this matter. We look forward to your continued commitment to your studies and your presence in future classes.</p>
        <p>Best regards,</p>
        <p>College Management<br>Attendance Monitoring Team</p>
    </div>
</body>
</html>";

            return htmlContent;
        }

    }
}