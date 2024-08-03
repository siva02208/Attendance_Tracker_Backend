using AttendenceTracker.Helper;
using System.Threading.Tasks;

namespace AttendenceTracker.services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailrequest);

    }
}
