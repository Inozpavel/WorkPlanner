using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tasks.Api.Data
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Create(T element);

        public Task<T?> Find(Expression<Func<T, bool>> expression);

        public void Update(T element);

        public void Delete(T element);
    }
}