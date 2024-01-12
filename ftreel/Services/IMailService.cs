using ftreel.Entities;
using ftreel.Dto.error;

namespace ftreel.Services;

public interface IMailService
{
    void SendMail(User user, Document document);
}