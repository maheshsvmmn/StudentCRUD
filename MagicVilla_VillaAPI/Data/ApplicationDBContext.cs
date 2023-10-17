using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Madhav", Class = 10, Weight = 50.4 , CreatedAt = DateTime.Now },
                new Student { Id = 2, Name = "Suresh", Class = 5, Weight = 30.6 , CreatedAt = DateTime.Now },
                new Student { Id = 3, Name = "Deepak", Class = 7, Weight = 40.4 , CreatedAt = DateTime.Now },
                new Student { Id = 4, Name = "Hemant", Class = 12, Weight = 55 , CreatedAt = DateTime.Now },
                new Student { Id = 5, Name = "Shashank", Class = 3, Weight = 25.4  , CreatedAt = DateTime.Now}
                ) ;
        }
    }
}
