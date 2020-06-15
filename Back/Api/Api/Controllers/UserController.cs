using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUserId")]
        public string GetUserId(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
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
