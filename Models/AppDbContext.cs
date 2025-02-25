using Microsoft.EntityFrameworkCore;

namespace WebServer.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> profiles { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<Question> Question { get; set; } = null!;
        public DbSet<Answer> Answer { get; set; } = null!;
    }
}
