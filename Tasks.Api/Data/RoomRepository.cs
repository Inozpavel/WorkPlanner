using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class RoomRepository : Repository<Room>
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {
        }
    }
}