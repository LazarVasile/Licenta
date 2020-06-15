using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
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
                productToAdd.professorPrice = Convert.ToDouble(request.professorPrice);
                productToAdd.studentPrice = Convert.ToDouble(request.studentPrice);
                productToAdd.weight = Convert.ToInt32(Convert.ToString(request.weight));
                productToAdd.description = request.description;
                

                collection.InsertOneAsync(productToAdd);
                

                return "succes";
            }


        }

        // PUT: api/Product/5
        [HttpPut]
        public void Put([FromBody] IDictionary<string, string> request)
        {
            IMongoCollection<Product> collection = _productService.GetCollectionProduct();
            var filter = Builders<Product>.Filter.Eq("_id", request["id"]);
            if (request["professor_price"] != "")
            {
                var update = Builders<Product>.Update.Set("professorPrice", request["professor_price"]);
                collection.UpdateOne(filter, update);
            }
            if (request["student_price"] != "")
            {
                var update = Builders<Product>.Update.Set("studentPrice", request["student_price"]);
                collection.UpdateOne(filter, update);
            }
            if (request["weight"] != "")
            {
                var update = Builders<Product>.Update.Set("weight", request["weight"]);
                collection.UpdateOne(filter, update);
            }
            if (request["description"] != "")
            {
                var update = Builders<Product>.Update.Set("description", request["description"]);
                collection.UpdateOne(filter, update);
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
