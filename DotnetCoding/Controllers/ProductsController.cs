using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IProductService _productService;
        public readonly IApprovalQueue _approvalQueue;
        public ProductsController(IProductService productService, IApprovalQueue approvalQueue)
        {
            _productService = productService;
            _approvalQueue = approvalQueue;
        }

        /// <summary>
        /// Get the list of product
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var productDetailsList = await _productService.GetAllProducts();
           
            return Ok(productDetailsList);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        { 
            var product = await _productService.GetProductById(id);
            if(product == null) 
            { 
                return NotFound();
            }
            return Ok(product); 
        }

        [HttpGet("GetProductByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            /*
            var product = await _productService.GetProductByName(name);
            if (product == null)
            {
                return NotFound();
            } 
            */
            var products = await _productService.GetAllProducts();
            var result = products.Where(x => x.ProductName == name);
            return Ok(result);
        }


        [HttpGet("GetProductsByPrice/{lowPrice}/{highPrice}")]
        public async Task<IActionResult> GetProductByName(int lowPrice, int highPrice)
        {
            if (highPrice < lowPrice) { 
                return NoContent();
            }
            var products = await _productService.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }
            var result = products.Where(x => x.ProductPrice >= lowPrice && x.ProductPrice <= highPrice); 
            return Ok(result);
        }


        [HttpGet("GetProductsByDate/{earlyDate}/{lateDate}")]
        public async Task<IActionResult> GetProductByDate(string earlyDate, string lateDate)
        {
            DateTime early = DateTime.Parse(earlyDate);
            DateTime late = DateTime.Parse(lateDate);
            if (late < early)
            {
                return NoContent();
            }
            var products = await _productService.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }
            var result = products.Where(x => x.RequestDate >= early && x.RequestDate <= late);
            return Ok(result);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDetails product)
        {
            if (product == null) {
                return NotFound(); 
            }
            product.ProductStatus = "Create"; 
            if (product.ProductPrice > 5000) {
                _approvalQueue.Enqueue(product);
                return Ok(); 
            } 
            // await _productService.CreateProduct(product);
            // return Ok(product);
            try
            {
                await _productService.CreateProduct(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDetails product)
        {
            var oldProduct = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            product.ProductStatus = "Update";
            product.Id = id; 
            if (product.ProductPrice > 5000)
            {
                _approvalQueue.Enqueue(product);
                return Ok();
            }
            if (product.ProductPrice > oldProduct.ProductPrice * 0.5) 
            {
                _approvalQueue.Enqueue(product);
                return Ok();
            }

            await _productService.UpdateProduct(id, product); 
            return Ok(product);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        { 
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            product.ProductStatus = "Delete";
            _approvalQueue.Enqueue(product);
            return Ok(product);
        }

        [HttpGet("GetPeekProduct")]
        public async Task<IActionResult> GetPeekProduct()
        {
            var product = _approvalQueue.GetPeekProduct();
            return Ok(product); 
        }

        [HttpGet("GetAllInQueue")]
        public async Task<IActionResult> GetAllInQueue()
        { 
            var productList = _approvalQueue.GetAllProducts();
            return Ok(productList);
        }

        [HttpPost("HandleApproval/{approval}")]
        public async Task<IActionResult> HandleApproval(bool approval)
        {
            var peek = _approvalQueue.GetPeekProduct();
            if (peek == null) {
                return Ok(); 
            }
            if (approval) {
                if (peek.ProductStatus == "Create")
                {
                    await _productService.CreateProduct(peek);
                }
                else if (peek.ProductStatus == "Update")
                {
                    await _productService.UpdateProduct(peek.Id, peek);
                }
                else {
                    await _productService.DeleteProduct(peek.Id); 
                }
            } 
            _approvalQueue.Dequeue();
            return Ok(peek); 
        }
    }  
}
