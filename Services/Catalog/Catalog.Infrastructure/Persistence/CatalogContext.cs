using Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogContext: ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {

            var client = new MongoClient(/*"mongodb://localhost:27017"*/configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

            Products = database.GetCollection<Product>(configuration["DatabaseSettings:CollectionName"]);
            CatalogContextSeed.SeedData(Products);
        }

        
        public IMongoCollection<Product> Products { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {

            var currentDate = DateTime.Now;
            var createdBy = "swn";

            var productList = await Products.Find(p => true).ToListAsync(cancellationToken);
            foreach (var product in productList)
            {
                if (product.CreatedDate == default(DateTime))
                {
                    product.CreatedDate = currentDate;
                    product.CreatedBy = createdBy;
                }
                else
                {
                    product.LastModifiedDate = currentDate;
                    product.LastModifiedBy = createdBy;
                }
                await Products.ReplaceOneAsync(p => p.Id == product.Id, product, new ReplaceOptions { IsUpsert = true }, cancellationToken);
            }

            return productList.Count;
        }
    }
}
