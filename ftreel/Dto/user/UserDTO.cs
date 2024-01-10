using ftreel.Dto.category;
using ftreel.Entities;

namespace ftreel.Dto.user;

public class UserDTO
{
    public int? Id { get; set; }
    public string? Mail { get; set; }
    public IList<string>? Roles { get; set; }
    

    public UserDTO(User? user)
    {
        Id = user?.Id;
        Mail = user?.Mail;
        Roles = user?.Roles;
    }
}