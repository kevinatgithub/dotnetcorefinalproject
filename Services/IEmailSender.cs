using Services.DTO;
using System.Threading.Tasks;

namespace Services
{
    public interface IEmailSender
    {
        void Send(EmailMessage email);
    }
}
