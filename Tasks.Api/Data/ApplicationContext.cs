using Microsoft.EntityFrameworkCore;
using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<UserInTheRoom> UsersInRoom { get; set; }

        public DbSet<RoomRole> RoomRoles { get; set; }

        public DbSet<RoomTask> RoomTasks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserInTheRoom> UsersInTheRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder
            .Entity<UserInTheRoom>()
            .HasKey(x => new {x.RoomId, RoomUserId = x.UserId});
    }
}