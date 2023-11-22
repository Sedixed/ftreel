namespace ftreel.Entities;

public class User
{
    public int Id { get; set; }
    
    public required string Username { get; set; }
    public string? Password { get; set; }
    
    public IList<string>? Roles { get; set; }
}