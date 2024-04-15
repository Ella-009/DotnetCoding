using DotnetCoding.Core.Models;
using System.Collections;

namespace DotnetCoding.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<ProductDetails>
    {

        Task<ProductDetails> GetProductById(int id);
        Task<ProductDetails> GetProductByName(string name); 
        Task<ProductDetails> CreateProduct(ProductDetails product);
        Task<ProductDetails> UpdateProduct(int id, ProductDetails product); 
        Task<ProductDetails> DeleteProduct(int id); 

    }
}
