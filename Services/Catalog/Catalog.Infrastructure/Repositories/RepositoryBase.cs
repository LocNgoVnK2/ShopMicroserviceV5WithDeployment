using Catalog.Application.Contracts.Persistence;
using Catalog.Domain.Common;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    /*
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        private readonly ICatalogContext _context;
        public RepositoryBase(ICatalogContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Products.InsertOneAsync(entity);

            return await _context
                       .Products
                       .Find(p => p.Id == entity.Id)
                       .FirstOrDefaultAsync();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
    */
}
