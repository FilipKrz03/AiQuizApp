using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Query();
        IQueryable<T> GetByIdQuery(Guid id);
        void Insert(T entity);
        void DeleteEntity(T entity);
        void AddRange(IEnumerable<T> entities);
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> EntityExistAsync(Guid id);
        Task SaveChangesAsync();
    }
}
