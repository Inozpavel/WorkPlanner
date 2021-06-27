using System.Threading.Tasks;
using Tasks.Api.Data.Interfaces;
using Tasks.Api.Data.Repositories;

namespace Tasks.Api.Data
{
    public class UnitOfWork
    {
        private readonly ApplicationContext _context;

        private IRoomRepository? _roomRepository;

        private IRoomRoleRepository? _roomRoleRepository;

        private IRoomTaskRepository? _roomTaskRepository;

        public UnitOfWork(ApplicationContext context) => _context = context;

        public IRoomRepository RoomRepository => _roomRepository ??= new RoomRepository(_context);

        public IRoomRoleRepository RoomRoleRepository => _roomRoleRepository ??= new RoomRoleRepository(_context);

        public IRoomTaskRepository RoomTaskRepository => _roomTaskRepository ??= new RoomTaskRepository(_context);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}