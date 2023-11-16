using Microsoft.EntityFrameworkCore;

namespace ftreel.DATA
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }
    }
}