using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/usermenu")]
    [ApiController]
    public class UserMenuController : ControllerBase
    {
        public readonly DatabaseService _myService;

        public UserMenuController (DatabaseService myService)
        {
            _myService = myService;
        }

        // GET: api/UserMenu
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //GET: api/UserMenu/5
        [HttpGet("{date}", Name = "GetProducts")]
        public List<Product> GetProducts(DateTime date)
        {
            Console.Write("getproducts");
            var myProducts = _myService.GetMenu(date);
            return myProducts;
        }

        //POST: api/UserMenu
        //[HttpPost]
        // public List<Product> Post([FromBody] Dictionary<string, DateTime> value)
        // {

        //     //var myProducts = _myService.GetMenu(value["myDate"]);
        //     //return myProducts; 
        //     var myProducts = _myService.GetMenu(value["myDate"]);
        //     return myProducts;
        //     //return _myService.GetMenu(value["myDate"]);   
        // }

        // PUT: api/UserMenu/5
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
