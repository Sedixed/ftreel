namespace ftreel.Dto.user;

public class SaveUserDTO
{
    public string? Mail { get; set; }
    public string? Password { get; set; }
    public IList<string>? Roles { get; set; }
}