using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class RoomRoleRepository : Repository<RoomRole>
    {
        public RoomRoleRepository(ApplicationContext context) : base(context)
        {
        }
    }
}