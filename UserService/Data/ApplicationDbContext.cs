using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserService.Models;

namespace UserService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "user1", Password = "password1", RefreshToken = "", RefreshTokenExpiryTime = DateTime.Now.AddDays(-1) },
                new User { Id = 2, Username = "user2", Password = "password2", RefreshToken = "", RefreshTokenExpiryTime = DateTime.Now.AddDays(-1) }
            );
        }
    }
}
