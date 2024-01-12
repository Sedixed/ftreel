using System.Net.Mail;
using ftreel.Constants;
using ftreel.Entities;
using ftreel.Dto.error;
using ftreel.Utils;

namespace ftreel.Services;

public class MailService : IMailService
{
    private readonly ILogger _logger;

    public MailService(ILogger<DocumentService> logger)
    {
        _logger = logger;
    }

    public void SendMail(User user, Document document)
    {
        var smtpServer = "localhost";
        var smtpPort = 1025;

        var mailSingleton = MailSingleton.GetInstanceMailSingleton();
        mailSingleton.Document = document;
        mailSingleton.ReplacePlaceHolders();

        var mailMessage = new MailMessage(SystemMail.FTREEL_SYSTEM_MAIL, user.Mail,
            mailSingleton.Subject, mailSingleton.Body);
        var smtpClient = new SmtpClient(smtpServer, smtpPort);
        
        try
        {
            smtpClient.Send(mailMessage);
            _logger.LogInformation("Mail successfully sent to " + user.Mail + " at {Time}.", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Failed to send mail to " + user.Mail + " at {Time}.", DateTime.UtcNow);
        }
        finally
        {
            mailMessage.Dispose();
            smtpClient.Dispose();
        }
    }
}