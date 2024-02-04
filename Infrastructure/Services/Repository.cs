using Domain.Common;
using Infrastructure.DbContexts;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly QuizApplicationDbContext _context;

        public Repository(QuizApplicationDbContext context) => _context = context;

        public IQueryable<T> Query()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetByIdQuery(Guid id)
        {
            return _context.Set<T>()
                .Where(e => e.Id == id);
        }

        public void Insert(T entity)
        {
            _context.Set<T>()
                .Add(entity);
        }

        public void DeleteEntity(T entity)
        {
            _context.Set<T>()
                .Remove(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EntityExistAsync(Guid id)
        {
            return await _context.Set<T>()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
