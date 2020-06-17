using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Authorize]
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
        [HttpGet]
        public ActionResult<List<User>> GetUsers() =>
            _userService.GetUsers();

        //// GET: api/User/5
        //[HttpGet("{id}", Name = "GetUserId")]
        //public string GetUserId(int id)
        //{
        //    return "value";
        //}

        //// POST: api/User
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT: api/User/5
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
                if (user.role != "admin")
                {
                    var filter = Builders<User>.Filter.Eq("email", email);
                    var update = Builders<User>.Update.Set("role", "admin");
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

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
