using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Services
{
    public class ProductService : IProductService
    {
        public IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDetails>> GetAllProducts()
        {
            var productDetailsList = await _unitOfWork.Products.GetAll();
            return productDetailsList;
        }

        public async Task<ProductDetails> GetProductById(int id)
        {
            var product = await _unitOfWork.Products.GetProductById(id);
            return product; 
        }

        public async Task<ProductDetails> GetProductByName(string name)
        {
            var product = await _unitOfWork.Products.GetProductByName(name);
            return product;
        }

        public async Task<ProductDetails> CreateProduct(ProductDetails product)
        {
            try {
                await _unitOfWork.Products.CreateProduct(product);
                await _unitOfWork.SaveAsync();
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
           
            return product;
        }

        public async Task<ProductDetails> UpdateProduct(int id, ProductDetails product)
        {
            await _unitOfWork.Products.UpdateProduct(id, product);
            await _unitOfWork.SaveAsync();
            return product;
        }

        public async Task<ProductDetails> DeleteProduct(int id) { 
            var product = await GetProductById(id);
            await _unitOfWork.Products.DeleteProduct(id);
            await _unitOfWork.SaveAsync();
            return product;
        }

    }
}
