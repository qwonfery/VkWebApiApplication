using Microsoft.EntityFrameworkCore;
using VkWebApiApplication.Models;

namespace VkWebApiApplication
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;
        public DbSet<UserState> UserStates { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Login = "Riden", Password = "Jack the Reaper" },
                    new User { Id = 2, Login = "Sam", Password = "desperados" },
                    new User { Id = 3, Login = "Armstrong", Password = "nanomachines" }
            );

            modelBuilder.Entity<UserState>().HasData(
                    new UserState {Id = 1, Code = UserStateCode.Active, Description = "user is active" },
                    new UserState { Id = 2, Code = UserStateCode.Blocked, Description = "user is blocked" }
            );

            modelBuilder.Entity<UserGroup>().HasData(
                    new UserGroup { Id = 1, Code = UserGroupCode.User, Description = "user is user" },
                    new UserGroup { Id = 2, Code =  UserGroupCode.Admin, Description = "user is admin" }
            );
        }


    }
}
