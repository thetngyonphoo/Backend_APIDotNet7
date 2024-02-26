using Microsoft.EntityFrameworkCore;
using WebAPIDotNet7.Models;

namespace WebAPIDotNet7.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected ApplicationDBContext()
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<AuthenticationSetting> AuthenticationSetting { get; set; }
        public virtual DbSet<RoleAuthentication> RoleAuthentication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleAuthentication>()
                .HasKey(k => new { k.RoleId, k.AuthenticationSettingId });
        }
    }
}
