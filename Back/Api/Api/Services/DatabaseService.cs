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

        public DatabaseService()
        {
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0-vu6o6.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("gaudeamus");

            _users = database.GetCollection<User>("userslist");

            _products = database.GetCollection<Product>("products");

            _menus = database.GetCollection<Menu>("menus");
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
            Console.WriteLine(myDate);
            //return _menus.Find(menus => DateTime.Compare(menus.dateMenu, myDate) == 0).ToList();

            List<Menu> my_ids = _menus.Find(menu => menu.dateMenu == myDate).ToList();
            List<Product> myProducts = new List<Product>();
            
            for (int i = 0; i < my_ids.Count; i++)
            {
                List<Product> myProduct = _products.Find(p => p._id == my_ids[i].productId).ToList();
                myProducts.Add(myProduct[0]);
            }


            return myProducts;
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
