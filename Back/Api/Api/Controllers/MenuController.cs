using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public readonly DatabaseService _menuService;
        
        public MenuController (DatabaseService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menu
        [HttpGet]
        public ActionResult<List<Menu>> GetMenus() =>
            _menuService.GetMenus();


        // GET: api/Menu/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Menu
        [HttpPost]
        public string Post([FromBody] CreateMenu request)
        {
            List<Menu> menus = _menuService.GetMenus();
            Console.WriteLine("dsa");
            Console.WriteLine(request.products.GetType());
            IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
            //Console.WriteLine(value.dateMenu);
            //var number = menus.Count;
            foreach(KeyValuePair<string, int> item in request.products)
            {
                var newMenu = new Menu();
                newMenu._id = menus.Count + 1;
                newMenu.dateMenu = request.dateMenu;
                newMenu.productId = Int32.Parse(item.Key);
                Console.WriteLine(newMenu.productId);
                
                newMenu.productCantity = item.Value;
                collection.InsertOneAsync(newMenu);
            }
            //for (int i = 0; i <value.listIds.Count; i++)
            //{
            //    var newMenu = new Menu();
            //    newMenu._id = menus.Count + i;
            //    newMenu.dateMenu = value.dateMenu;
            //    newMenu.productId = value.listIds[i];
            //    collection.InsertOneAsync(newMenu);

            //}

            return "true";

        }

        // PUT: api/Menu/5
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
