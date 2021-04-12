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

        public DbSet<UserInRoom> UsersInRoom { get; set; }

        public DbSet<RoomRole> RoomRoles { get; set; }

        public DbSet<RoomTask> RoomTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRoom>().HasKey(x => new {x.RoomId, x.UserId});
        }
    }
}