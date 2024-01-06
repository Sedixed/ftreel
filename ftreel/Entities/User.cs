using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public required string Username { get; set; }
    
    public string? Password { get; set; }
    
    public IList<string>? Roles { get; set; }
}