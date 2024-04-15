using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces
{
    public interface IApprovalQueue
    {
        void Enqueue(ProductDetails product);
        void Dequeue(); 
        List<ProductDetails> GetAllProducts();
        ProductDetails GetPeekProduct();
    }
}
