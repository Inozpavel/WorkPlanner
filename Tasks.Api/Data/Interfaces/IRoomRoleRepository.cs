using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Api.Entities;

namespace Tasks.Api.Data.Interfaces
{
    public interface IRoomRoleRepository : IRepository<RoomRole>
    {
        Task<IEnumerable<RoomRole>> FindAllRoles();

        Task<RoomRole> FindWithName(string roleName);
    }
}