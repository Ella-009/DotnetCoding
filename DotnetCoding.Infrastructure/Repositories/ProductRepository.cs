using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<ProductDetails>, IProductRepository
    {
        protected readonly DbContextClass _dbContext;

        public ProductRepository(DbContextClass dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductDetails> GetProductById(int id)
        { 
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) {
                throw new ArgumentNullException(nameof(product));
            }
            return product; 
        }

        public async Task<ProductDetails> GetProductByName(string name)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductName == name);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            return product;
        }

        public async Task<ProductDetails> CreateProduct(ProductDetails product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            } 
            
            await _dbContext.Products.AddAsync(product);
            return product; 
        }

        public async Task<ProductDetails> UpdateProduct(int id, ProductDetails product)
        {
            var oldProduct = await _dbContext.Products.FindAsync(id);
            if (oldProduct == null) {
                throw new ArgumentNullException(nameof(product));
            }
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            oldProduct.ProductName = product.ProductName; 
            oldProduct.ProductDescription = product.ProductDescription; 
            oldProduct.ProductPrice = product.ProductPrice;
            oldProduct.ProductStatus = "Update"; 
            oldProduct.RequestReason = product.RequestReason;
            oldProduct.RequestDate = product.RequestDate;

            return oldProduct;
        }

        public async Task<ProductDetails> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _dbContext.Products.Remove(product); 
            return product;
        }
    }
}
