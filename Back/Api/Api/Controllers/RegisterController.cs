using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Api.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly DatabaseService _userService;

        public RegisterController(DatabaseService userService)
        {
            _userService = userService;
        }

        // GET: api/Register
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Register/5
        [HttpGet("{id}", Name = "GetRegister")]
        public string Get(int id)
        {
            return "value";
        }
        // PUT: api/Register/5
        [HttpPut]
        public IDictionary<String, String> Put([FromBody]  Register request)
        {
            List<User> users = _userService.GetUsers();
            IMongoCollection<User> collection = _userService.GetCollectionUser();
            IDictionary<String, String> dict = new Dictionary<String, String>();

            if (users.Exists(x => x.email == request.email) == true)//&& users.Exists(y => x.password == request.password) == true)
            {
                dict.Add("response", "false");
                return dict;
            }
            else
            {
                /*var document = new BsonDocument()
                {

                    { "id" , new BsonInt32(users[users.Count]._id + 1) },
                    { "email", new BsonString(request.email) },
                    { "username", new BsonString(request.username)},
                    { "password", new BsonString(request.password) }
                };
                */
                var user_add = new User();
                user_add._id = users[users.Count - 1]._id + 1;
                user_add.email = request.email;
                user_add.password = _userService.ComputeSha256(request.password);
                user_add.role = "user";
                user_add.type = "student";
                collection.InsertOneAsync(user_add);
                //adaugare user in baza de date
                dict.Add("response", "true");
                return dict;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
