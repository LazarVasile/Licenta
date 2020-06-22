using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using System;

namespace Api.Controllers
{
    //[Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _userService;

        public UserController(DatabaseService userService)
        {
            _userService = userService;
        }
        // GET: api/User

        [HttpGet("{id}", Name = "GetProductsByUserId")]
        public List<Product> Get(int id)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            List<Product> myProductsList1 = _userService.getIdProductsByIdUser(id);
            List<Product> myProductsList2 = _userService.GetMenu(dNow);
            List<Product> myProducts = new List<Product>();
            for (int i = 0; i < myProductsList1.Count; i++)
            {
                if (myProductsList2.Exists(x => x._id == myProductsList1[i]._id))
                {
                    myProducts.Add(myProductsList1[i]);
                }
            }

            return myProducts;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers() =>
            _userService.GetUsers();


        //Register
        [HttpPost]
        public IDictionary<string, string> Post([FromBody]  IDictionary<string, string> request)
        {
            List<User> users = _userService.GetUsers();
            IMongoCollection<User> collection = _userService.GetCollectionUser();
            IDictionary<string, string> dict = new Dictionary<string, string>();

            if (users.Exists(x => x.email == request["email"]) == true)//&& users.Exists(y => x.password == request.password) == true)
            {
                dict.Add("response", "false");
                return dict;
            }
            else
            {
                try
                {
                    if (request["email"].Contains("@info.uaic.ro"))
                    {
                        var user_add = new User();
                        user_add._id = users[users.Count - 1]._id + 1;
                        user_add.email = request["email"];
                        user_add.password = _userService.ComputeSha256(request["password"]);
                        user_add.role = "user";
                        if (request["type"] == "professor")
                        {
                            user_add.type = "professor";
                        }
                        else if (request["type"] == "student")
                        {
                            user_add.type = "student";
                        }
                        else
                        {
                            user_add.type = "normal";
                        }

                        collection.InsertOneAsync(user_add);
                        //adaugare user in baza de date
                        dict.Add("response", "true");
                        return dict;
                    }
                    else
                    {
                        dict.Add("response", "false");
                        return dict;
                    }
                }
                catch
                {
                    dict.Add("response", "false");
                    return dict;
                }
            }
        }


        [HttpPut]
        public IDictionary<string, string> Put([FromBody] IDictionary<string, string> request)
        {
            IMongoCollection<User> collection = _userService.GetCollectionUser();
            var email = request["email"];
            List<User> users = _userService.GetUsers();
            User user = new User();
            var dict = new Dictionary<string, string> { };
            try
            {
                user = users.Find(user => user.email == email);
                if (user.type != "normal")
                {
                    var filter = Builders<User>.Filter.Eq("email", email);
                    var update = Builders<User>.Update.Combine(Builders<User>.Update.Set("role", "staff"), Builders<User>.Update.Set("type", "admin"));
                    collection.UpdateOne(filter, update);
                    dict["response"] = "true";
                    return dict;
                }
                else
                {
                    dict["response"] = "false";
                    return dict;
                }
            }
            catch
            {
                dict["response"] = "false";
                return dict;
            }
        }
    }
}
