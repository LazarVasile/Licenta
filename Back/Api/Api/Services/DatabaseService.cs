using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Globalization;


namespace Api
{
    public class DatabaseService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Menu> _menus;
        private readonly IMongoCollection<BuyProducts> _codes;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0-vu6o6.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("gaudeamus");

            _users = database.GetCollection<User>("userslist");

            _products = database.GetCollection<Product>("products");

            _menus = database.GetCollection<Menu>("menus");

            _codes = database.GetCollection<BuyProducts>("buy_products");
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

        public List<Product> GetMenu(DateTime myDate)
        {
            Console.WriteLine("GetProducts: " + myDate);
            //return _menus.Find(menus => DateTime.Compare(menus.dateMenu, myDate) == 0).ToList();

            List<Menu> myIds = _menus.Find(menu => menu.dateMenu == myDate).ToList();
            List<Product> myProducts = new List<Product>();
            
            for (int i = 0; i < myIds.Count; i++)
            {
                List<Product> myProduct = _products.Find(p => p._id == myIds[i].productId).ToList();
                myProducts.Add(myProduct[0]);
            }


            return myProducts;
        }

        public List<BuyProducts> GetCodesByDate(DateTime myDate)
        {
            List<BuyProducts> myCode = _codes.Find(c => c.date == myDate).ToList();
            return myCode;
        }

        public List<BuyProducts> GetCodesByCodeAndDate(int code, DateTime myDate)
        {
            List<BuyProducts> myCode = _codes.Find(c => c.code == code && c.date == myDate).ToList();
            return myCode;
        }

        public List<BuyProducts> GetCodes()
        {
            List<BuyProducts> myCode = _codes.Find(x => x._id > 0).ToList();
            return myCode;
        }


        public List<Menu> GetMenusByDate(DateTime myDate)
        {
            Console.WriteLine("GetMenusByDate: " + myDate);
            List<Menu> myMenus = _menus.Find(menu => menu.dateMenu == myDate).ToList();

            return myMenus;
        }

        public int GetQuantity(int idProduct, DateTime myDate)
        {
            Console.WriteLine(myDate);
            Menu product = _menus.Find(menu => menu.productId == idProduct && menu.dateMenu == myDate).ToList()[0];
            int quantity = product.productCantity;
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

        public IMongoCollection<BuyProducts> GetCollectionCodes()
        {
            return _codes;
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
