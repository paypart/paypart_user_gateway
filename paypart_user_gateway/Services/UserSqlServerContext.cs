using Microsoft.EntityFrameworkCore;
using paypart_user_gateway.Models;

namespace paypart_user_gateway.Services
{
    public class UserSqlServerContext : DbContext
    {
        public UserSqlServerContext(DbContextOptions<UserSqlServerContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
        }
    }
}
