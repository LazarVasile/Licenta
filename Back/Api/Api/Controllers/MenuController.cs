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
        [HttpGet("{date}", Name = "GetMenusByDate")]
        public List<Menu> GetMenusByDate(DateTime date)
        {
            Console.WriteLine(date);
            var myMenu =_menuService.GetMenusByDate(date);
            return myMenu;

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
            var number = menus.Count;
            DateTime myDate = Convert.ToDateTime(request.dateMenu.ToString("dd-MM-yyyy"));
            foreach(KeyValuePair<string, int> item in request.products)
            {
                number++;
                Console.WriteLine(item.Key);
                var newMenu = new Menu();
                newMenu._id = number;
                newMenu.dateMenu = myDate;
                newMenu.productId = Int32.Parse(item.Key);
                
                newMenu.productCantity = item.Value;
                try
                {
                    collection.InsertOneAsync(newMenu);
                } catch (Exception)
                {
                    Console.WriteLine("Nu s-a putut adauga produs in meniu!");
                }
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
        [HttpPut]
        public void Put([FromBody] List<BuyProducts> request)
        {
            for (int i = 0; i < request.Count; i++)
            {
                
                IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
                var quantity = _menuService.GetQuantity(request[i].idProduct, request[i].date);
                Console.WriteLine("Vechea cantitate: " + quantity);
                var newQuantity = quantity - request[i].quantity;
                var arrayFilter = Builders<Menu>.Filter.Eq("product_id", request[i].idProduct) & Builders<Menu>.Filter.Eq("date_menu", request[i].date);
                var update = Builders<Menu>.Update.Set("product_cantity", newQuantity);
                try { 
                    collection.UpdateOne(arrayFilter, update);
                }
                catch (Exception)
                {
                    Console.WriteLine("Nu se poate face update");
                }
                var quantity1 = _menuService.GetQuantity(request[i].idProduct, request[i].date);

                Console.WriteLine("Noua cantitate: " + quantity1);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
