﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            DateTime myDate = Convert.ToDateTime(request.dateMenu.ToString("yyyy-MM-dd"));
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
        public void Put([FromBody] IDictionary<String, double> request)
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

            foreach (KeyValuePair<string, double> item in request)
            {
                if (item.Key != "total_price")
                {
                    var q = Convert.ToInt32(item.Value);
                    history.nameProductsAndAmounts[products[item.Key.ToString()]] = q;

                    var quantity = _menuService.GetQuantity(Convert.ToInt32(item.Key), dNow);
                    Console.WriteLine("Vechea cantitate: " + quantity);
                    var newQuantity = quantity - q;
                    var arrayFilter = Builders<Menu>.Filter.Eq("product_id", Int32.Parse(item.Key)) & Builders<Menu>.Filter.Eq("date_menu", dNow);
                    var update = Builders<Menu>.Update.Set("product_cantity", newQuantity);
                    try
                    {
                        collection.UpdateOne(arrayFilter, update);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Nu se poate face update");
                    }
                    var quantity1 = _menuService.GetQuantity(Int32.Parse(item.Key), dNow);

                    Console.WriteLine("Noua cantitate: " + quantity1);
                }
            }

            collection2.InsertOneAsync(history);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
