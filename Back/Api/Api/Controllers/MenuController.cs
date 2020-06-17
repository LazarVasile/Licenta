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
    //[Authorize]
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
        public Menu GetMenuByDate(DateTime date)
        {
            Menu myMenu =_menuService.GetLastMenuByDate(date);
            return myMenu;

        }

        // POST: api/Menu
        [HttpPost]
        public IDictionary<string, string> Post([FromBody] CreateMenu request)
        {
            List<Menu> menus = _menuService.GetMenus();
            Console.WriteLine("dsa");
            Console.WriteLine(request.products.GetType());
            IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
            //Console.WriteLine(value.dateMenu);
            var number = menus.Count;
            DateTime myDate = Convert.ToDateTime(request.dateMenu.ToString("yyyy-MM-dd"));
            var newMenu = new Menu();
            newMenu._id = number + 1;
            newMenu.dateMenu = myDate;
            newMenu.productsIdAndAmounts = new Dictionary<String, int> { };
            var dict = new Dictionary<string, string> { };
            foreach(KeyValuePair<string, int> item in request.products)
            {
                newMenu.productsIdAndAmounts[item.Key] = item.Value;
            }
            try
            {
                collection.InsertOneAsync(newMenu);
                dict["response"] = "true";
                return dict;
            } catch 
            {
                dict["response"] = "false";
                return dict;
            }
          
            

        }

        public static int RandomCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            string stringCode = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int code = Int32.Parse(stringCode);

            return code;
        }
        // PUT: api/Menu/5
        [HttpPut]
        public string Put([FromBody] IDictionary<String, double> request)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
            IMongoCollection<History> collection2 = _menuService.GetCollectionHistory();
            int code = RandomCode();

            List<History> histories = _menuService.getHistories();
            History history = new History();
            while (histories.Exists(x => x._id == code) == true)
            {
                code = RandomCode();
            }
            history._id = code;
            history.date = dNow;
            IDictionary<string, string> products = _menuService.GetProductsById();
            history.nameProductsAndAmounts = new Dictionary<String, int> { };
            history.totalPrice = request["total_price"];
            Menu menu = _menuService.GetLastMenuByDate(dNow);
            IDictionary<string, int> copy =  new Dictionary<string, int>(menu.productsIdAndAmounts);

            foreach (KeyValuePair<string, double> item in request)
            {
                if (item.Key != "total_price")
                {
                    history.nameProductsAndAmounts[products[item.Key]] = Convert.ToInt32(item.Value); 
                    Console.WriteLine(menu.productsIdAndAmounts[item.Key]);
                    copy[item.Key] -= Convert.ToInt32(item.Value);
                    Console.WriteLine(menu.productsIdAndAmounts[item.Key] + " " + copy[item.Key]);

                }
            }
           
            var arrayFilter = Builders<Menu>.Filter.Eq("dateMenu", dNow) & Builders<Menu>.Filter.Eq("productsIdAndAmounts", menu.productsIdAndAmounts);
            var update = Builders<Menu>.Update.Set("productsIdAndAmounts", copy);

            try
            {
                collection.UpdateOne(arrayFilter, update);
            }
            catch (Exception)
            {
                Console.WriteLine("Nu se poate face update");
            }

            collection2.InsertOneAsync(history);
            return "succes";

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
