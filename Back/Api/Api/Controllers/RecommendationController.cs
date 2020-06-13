using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/products/recommendation")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        public readonly DatabaseService _recService;
        // GET: api/Recommendation
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public RecommendationController(DatabaseService recService)
        {

            _recService = recService;
        }
        // GET: api/Recommendation/5
        [HttpGet("{id}", Name = "GetProductsByUserId")]
        public List<Product> Get(int id)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            List<Product> myProductsList1 = _recService.getIdProductsByIdUser(id);
            List<Product> myProductsList2 = _recService.GetMenu(dNow);
            List<Product> myProducts = new List<Product>();
            for(int i = 0; i < myProductsList1.Count; i++)
            {
               if(myProductsList2.Exists(x => x._id == myProductsList1[i]._id)){
                    myProducts.Add(myProductsList1[i]);
                }
            }

            return myProducts;
        }

        // POST: api/Recommendation
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Recommendation/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
