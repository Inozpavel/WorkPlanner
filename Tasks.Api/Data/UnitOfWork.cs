using System.Threading.Tasks;

namespace Tasks.Api.Data
{
    public class UnitOfWork
    {
        private readonly ApplicationContext _context;

        private RoomRepository? _roomRepository;

        private RoomRoleRepository? _roomRoleRepository;

        private RoomTaskRepository? _roomTaskRepository;

        public UnitOfWork(ApplicationContext context) => _context = context;

        public RoomRepository RoomRepository => _roomRepository ??= new RoomRepository(_context);

        public RoomRoleRepository RoomRoleRepository => _roomRoleRepository ??= new RoomRoleRepository(_context);

        public RoomTaskRepository RoomTaskRepository => _roomTaskRepository ??= new RoomTaskRepository(_context);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}