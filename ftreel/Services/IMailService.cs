using ftreel.Entities;

namespace ftreel.Services;

public interface IMailService
{
    void SendMail(User user);
}