using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;


namespace Api.Controllers
{
    [Authorize]
    [Route("api/users/resetpassword")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly DatabaseService _rpService;

        public ResetPasswordController(DatabaseService rpService)
        {
            _rpService = rpService;
        }
        // GET: api/ResetPassword
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/ResetPassword/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/ResetPassword
        [HttpPost]
        public IDictionary<String, String> Post([FromBody] IDictionary<string, string> request)
        {
            
            try
            {
                IMongoCollection<User> collection = _rpService.GetCollectionUser();
                string password = _rpService.ComputeSha256(request["password"]);
                string token = request["token"];
                var filter = Builders<User>.Filter.Eq("token", token);
                var update = Builders<User>.Update.Set("password", password);
                collection.UpdateOne(filter, update);
                Dictionary<string, string> dict = new Dictionary<string, string> { };
                dict["response"] = "true";
                return dict;
            }
            catch (Exception)
            {
                Console.WriteLine("Nu s-a putut face update!");
                Dictionary<string, string> dict = new Dictionary<string, string> { };
                dict["response"] = "false";
                return dict;
            }
        }

        // PUT: api/ResetPassword/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
