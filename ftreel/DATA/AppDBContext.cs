using ftreel.Entities;
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
    }
}