using ftreel.Entities;

namespace ftreel.Dto.user;

public class UserDTO
{
    public int? Id { get; set; }
    public string? Username { get; set; }
    public IList<string>? Roles { get; set; }

    public UserDTO(int id, string username, IList<string> roles)
    {
        Id = id;
        Username = username;
        Roles = roles;
    }

    public UserDTO(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Roles = user.Roles;
    }
}