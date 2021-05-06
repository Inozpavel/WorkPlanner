using Microsoft.EntityFrameworkCore;

namespace Users.Api.Data
{
    public class DatabaseInitializer
    {
        private readonly ApplicationContext _applicationContext;

        public DatabaseInitializer(ApplicationContext applicationContext) => _applicationContext = applicationContext;

        public void Initialize() => _applicationContext.Database.Migrate();
    }
}