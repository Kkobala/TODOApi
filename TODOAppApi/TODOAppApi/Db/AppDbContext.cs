using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TODOAppApi.Db.Entities;

namespace TODOAppApi.Db
{
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        DbSet<SendMailRequestEntity> SendEmailRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<RoleEntity>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}
