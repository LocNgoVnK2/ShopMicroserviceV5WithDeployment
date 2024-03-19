using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Persistence
{
    public interface IProductRepository 
    {
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task<Product> AddAsync(Product entity);
        Task<bool> UpdateAsync(Product entity);
        Task<bool> DeleteAsync(string id);

    }
}
