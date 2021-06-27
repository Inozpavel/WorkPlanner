using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasks.Api.Data.Interfaces;
using Tasks.Api.Entities;

namespace Tasks.Api.Data.Repositories
{
    public class RoomRoleRepository : Repository<RoomRole>, IRoomRoleRepository
    {
        public RoomRoleRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RoomRole>> FindAllRoles() =>
            await _context.RoomRoles.ToListAsync();

        public async Task<RoomRole> FindWithName(string roleName) =>
            await _context.RoomRoles.FirstAsync(x => x.RoomRoleName == roleName);
    }
}