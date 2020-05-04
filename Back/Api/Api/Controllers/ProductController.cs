using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly DatabaseService _productService;

        public ProductController(DatabaseService productService)
        {
            _productService = productService;
        }
        // GET: api/Product
        [HttpGet]
        public ActionResult<List<Product>> GetProducts() =>
            _productService.GetProducts();
        

        // GET: api/Product/5
        [HttpGet("{id}", Name = "GetProductId")]
        public string GetProductId(int id)
        {
            return "value";
        }

        // POST: api/Product
        [HttpPost]
        public string Post([FromBody] AddProduct request)
        {
            List<Product> products = _productService.GetProducts();
            IMongoCollection<Product> collection = _productService.GetCollectionProduct();
            Console.WriteLine("dsadsa");
            Console.WriteLine(request);
            if (products.Exists( x => x.name == request.name ))
            {
                return "error";
            }
            else
            {
                var productToAdd = new Product();
                productToAdd._id = products[products.Count-1]._id + 1;
                productToAdd.name = request.name;
                productToAdd.category = request.category;
                productToAdd.professor_price = Convert.ToDouble(request.professor_price);
                productToAdd.student_price = Convert.ToDouble(request.student_price);
                productToAdd.weight = Convert.ToInt32(Convert.ToString(request.weight));
                productToAdd.description = request.description;
                

                collection.InsertOneAsync(productToAdd);
                

                return "succes";
            }


        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
