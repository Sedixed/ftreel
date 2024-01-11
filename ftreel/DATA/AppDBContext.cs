using ftreel.Constants;
using ftreel.Entities;
using ftreel.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ftreel.DATA;
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<Document> Documents { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category?> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var splitStringConverter =
            new ValueConverter<IList<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
        var stringListValueComparer = new ValueComparer<IList<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c
        );
        
        modelBuilder.Entity<User>().Property(nameof(User.Roles))
            .HasConversion(splitStringConverter)
            .Metadata.SetValueComparer(stringListValueComparer);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.ChildrenDocuments)
            .WithOne(d => d.Category)
            .HasForeignKey(d => d.CategoryId);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.ChildrenCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.FollowedCategories)
            .WithMany(c => c.Followers);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedDocuments)
            .WithOne(d => d.Author)
            .HasForeignKey(d => d.AuthorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedDocuments)
            .WithMany(d => d.Likes);
        
        // Insert users.
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Mail = "admin@ftreel.com",
                Password = PasswordManager.HashPassword("admin"),
                Roles = new List<string>()
                {
                    Roles.ROLE_ADMIN.ToString()
                }
            },
            new User
            {
                Id = 2,
                Mail = "user@ftreel.com",
                Password = PasswordManager.HashPassword("user"),
                Roles = new List<string>()
                {
                    Roles.ROLE_USER.ToString()
                }
            }
        );
    }
}