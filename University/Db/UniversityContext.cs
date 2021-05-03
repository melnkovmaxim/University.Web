using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Db
{
    public class UniversityContext: DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }

        public UniversityContext(DbContextOptions options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(c => c.Group);

            base.OnModelCreating(modelBuilder);
        }
    }
}
