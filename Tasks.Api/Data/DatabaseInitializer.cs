using System.Linq;
using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class DatabaseInitializer
    {
        private readonly ApplicationContext _database;

        public DatabaseInitializer(ApplicationContext database) => _database = database;

        public void Initialize()
        {
            EnsureRolesAdded();

            _database.SaveChanges();
        }

        private void EnsureRolesAdded()
        {
            if (!_database.RoomRoles.Any())
            {
                _database.RoomRoles.AddRange(Roles.AllRoles().Select(name => new RoomRole
                {
                    RoomRoleName = name
                }));
            }
        }
    }
}