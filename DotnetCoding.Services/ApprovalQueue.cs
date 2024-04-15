using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoding.Services
{
    public class ApprovalQueue : IApprovalQueue
    {
        public Queue<ProductDetails> queue { get; set; }
        public List<ProductDetails> list { get; set; }

        public ApprovalQueue()
        {
            queue = new Queue<ProductDetails>();
            list = new List<ProductDetails>(); 
        }
        public void Enqueue(ProductDetails product) 
        {
            queue.Enqueue(product);
            list.Add(product);
        }
        public void Dequeue()
        {
            if (queue.Count() == 0)
            {
                return;
            } 
            queue.Dequeue();
            list.RemoveAt(0); 
        }
        public List<ProductDetails> GetAllProducts()
        {
            return list; 
        }
        public ProductDetails GetPeekProduct()
        {
            if (queue.Count() == 0) {
                return null; 
            } 
            return queue.Peek();
        }
    }
}
