using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Mail { get; set; } = "";

    public string Password { get; set; } = "";

    public IList<string>? Roles { get; set; } = new List<string>();

    public virtual IList<Category> FollowedCategories { get; set; } = new List<Category>();

    public virtual IList<Document> CreatedDocuments { get; set; } = new List<Document>();

    public virtual IList<Document> LikedDocuments { get; set; } = new List<Document>();
}