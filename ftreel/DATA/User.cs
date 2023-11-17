namespace ftreel.DATA;

public class User
{
    public int Id { get; set; }
    
    public required string Username { get; set; }
    public string? Password { get; set; }

    public List<string>? Roles { get; set; }
}