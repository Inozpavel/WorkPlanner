using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        protected Repository(ApplicationContext context) => _context = context;

        public async Task<T> Create(T element)
        {
            var addedElement = await _context.Set<T>().AddAsync(element);
            return addedElement.Entity;
        }

        public async Task<T?> Find(Expression<Func<T, bool>> expression) =>
            await _context.Set<T>().FirstOrDefaultAsync(expression);

        public void Update(T element) => _context.Set<T>().Update(element);

        public void Delete(T element) => _context.Set<T>().Remove(element);
    }
}