using Catalog.Domain.Entities;

using MongoDB.Driver;
namespace Catalog.Infrastructure.Persistence
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
