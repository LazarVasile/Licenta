using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Drawing.Printing;
using System.Drawing;
using Eco.Report;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public class DatabaseService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Menu> _menus;
        private readonly IMongoCollection<Codes> _codes;
        private readonly IMongoCollection<History> _history;
        private IConfiguration _config;

        public DatabaseService(IConfiguration config)
        {
            _config = config;
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0-vu6o6.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("gaudeamus");

            _users = database.GetCollection<User>("userslist");

            _products = database.GetCollection<Product>("products");

            _menus = database.GetCollection<Menu>("menus");

            _codes = database.GetCollection<Codes>("codes");

            _history = database.GetCollection<History>("history");
        }

        public List<User> GetUsers()
        {
            return _users.Find(user => true).ToList();
        }

        public List<Product> GetProducts()
        {
            return _products.Find(product => true).ToList();
        }

        public List<Menu> GetMenus()
        {
            return _menus.Find(menus => true).ToList();
        }

        public IDictionary<string, string> GetProductsById()
        {
            List<Product> myProducts = GetProducts();
            IDictionary<string, string> myDictionary = new Dictionary<String, String> { };
            for(int i = 0; i < myProducts.Count; i++)
            {
                myDictionary[myProducts[i]._id.ToString()] = myProducts[i].name;
            }

            return myDictionary;
        }

        public List<History> getHistories()
        {
            return _history.Find(history => true).ToList();
        }

        public List<Product> GetMenu(DateTime myDate)
        {
            Console.WriteLine("GetProducts: " + myDate);
            //return _menus.Find(menus => DateTime.Compare(menus.dateMenu, myDate) == 0).ToList();

            List<Menu> myMenus = _menus.Find(menu => menu.dateMenu == myDate).ToList();
            Menu myMenu = myMenus.FindLast(menu => true);
            List<Product> myProducts = new List<Product>();

            if (myMenu != null)
            {

                foreach (KeyValuePair<string, int> item in myMenu.productsIdAndAmounts)
                {
                    List<Product> myProduct = _products.Find(p => p._id == Int32.Parse(item.Key)).ToList();
                    myProducts.Add(myProduct[0]);
                }
            }


            return myProducts;
        }

        public List<Codes> GetCodesByDate(DateTime myDate)
        {
            List<Codes> myCode = _codes.Find(c => c.date == myDate).ToList();
            return myCode;
        }

        public Codes GetCodesByCodeAndDate(int code, DateTime myDate)
        {
            Codes myCode = _codes.Find(c => c.code == code && c.date == myDate).ToList()[0];
            return myCode;
        }

        public List<Codes> GetCodes()
        {
            List<Codes> myCode = _codes.Find(x => true).ToList();
            return myCode;
        }



        public Menu GetLastMenuByDate(DateTime myDate)
        {
            List<Menu> myMenus = _menus.Find(menu => menu.dateMenu == myDate).ToList();
            Menu myMenu = myMenus.FindLast(menu => true);

            return myMenu;
        }

        public List<Product> getIdProductsByIdUser(int id)
        {
            List<Product> myProducts = new List<Product>();
            List<Codes> buyProductsList = _codes.Find(code => code.idUser == id).ToList();
            List<Product> myProductsFinal = new List<Product>();
            for (int i = 0; i < buyProductsList.Count; i++) { 
                foreach (KeyValuePair<string, int> item in buyProductsList[i].idProductsAndAmounts)
                {
                    List<Product> products = _products.Find(product => product._id == Int32.Parse(item.Key)).ToList();
                    if(products.Count > 0)
                    {
                        myProducts.Add(products[0]);
                    }
                }

            }
            Console.WriteLine(myProducts.Count);
            myProductsFinal.Add(myProducts[0]);
            if(myProducts.Count > 0)
            {
                for(int i = 1; i < myProducts.Count; i++)
                    if(myProductsFinal.Exists(x => x._id == myProducts[i]._id) == false)
                    {
                        myProductsFinal.Add(myProducts[i]);
                    }
            }

            return myProductsFinal;
            
        }

        public List<History> getHistoryByDate(DateTime myDate)
        {
            List<History> myHistories = _history.Find(history => history.date == myDate).ToList();
            return myHistories;
;        }

        public int GetQuantity(string idProduct, DateTime myDate)
        {
            Console.WriteLine(myDate);
            List<Menu> myMenus = _menus.Find(menu => menu.dateMenu == myDate).ToList();
            Menu myMenu = myMenus.FindLast(menu => true);
            int quantity = myMenu.productsIdAndAmounts[idProduct];
            return quantity;
        }

        public IMongoCollection<User> GetCollectionUser() { 
            return _users;
        }

        public IMongoCollection<Product> GetCollectionProduct()
        {
            return _products;
        }
        
        public IMongoCollection<Menu> GetCollectionMenu()
        {
            return _menus;
        }

        public IMongoCollection<Codes> GetCollectionCodes()
        {
            return _codes;
        }

        public IMongoCollection<History> GetCollectionHistory()
        {
            return _history;
        }

        public string GenerateJSONWebToken(string username, string type)
        {
            Console.WriteLine(type);
            var hours = 0;
            if(type == "web")
            {
                hours = 12;
            }
            else if (type == "password")
            {
                hours = 2;
            }
            else if (type == "mobile")
            {
                hours = 720;
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(hours),
                    signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ComputeSha256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }   
        }



    }
}
