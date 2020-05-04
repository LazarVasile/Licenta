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
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseService _userService;
        private IConfiguration _config;
        IDictionary<String, String> dict = new Dictionary<String, String>();

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
        public IDictionary<String, String> Post([FromBody]  Login request)
        {
            List<User> users = _userService.GetUsers();
            Console.WriteLine(request.email);
            //Console.WriteLine(request.password);
            // verificare daca emailul este in baza de date a facultatii
            if(users.Exists(x => x.email == request.email) == true && users.Exists(y => y.password == _userService.ComputeSha256(request.password)) == true)
            {
                User _user = users.Find(x => x.email == request.email);
                var tokenString = GenerateJSONWebToken(request.email);
                dict.Add("response", "true");
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

        private string GenerateJSONWebToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddSeconds(10),
                    signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
