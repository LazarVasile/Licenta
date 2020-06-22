using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/users/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseService _userService;
        private IConfiguration _config;

        public LoginController(DatabaseService userService, IConfiguration config)
        {
            _config = config;
            _userService = userService;
        }

        // GET: api/Test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Test/5
        [HttpGet("{id}", Name = "GetTest")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Test
        [HttpPost]
        public IDictionary<String, String> Post([FromBody] IDictionary<String, String> request)
        {
            
            List<User> users = _userService.GetUsers();
            String type = request["type"];
            IDictionary<String, String> dict = new Dictionary<String, String>();
            Console.WriteLine(_userService.ComputeSha256(request["password"]));
            if(users.Exists(x => x.email == request["email"] && x.password == _userService.ComputeSha256(request["password"]))){
                User _user = users.Find(x => x.email == request["email"]);
                Console.WriteLine("User id:" + _user._id);
                var tokenString = _userService.GenerateJSONWebToken(request["email"], type);
                dict.Add("response", "true");
                dict.Add("id_user", _user._id.ToString());
                dict.Add("role", _user.role);
                dict.Add("type", _user.type);
                dict.Add("token", tokenString);
                return dict;
            }
            else
            {
                dict.Add("response", "false");
                return dict;
            }
           
        }

        // PUT: api/Test/5
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
