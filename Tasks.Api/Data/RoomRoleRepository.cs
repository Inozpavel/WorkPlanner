using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasks.Api.Entities;

namespace Tasks.Api.Data
{
    public class RoomRoleRepository : Repository<RoomRole>
    {
        public RoomRoleRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RoomRole>> FindAllRoles() =>
            await _context.RoomRoles.ToListAsync();
    }
}