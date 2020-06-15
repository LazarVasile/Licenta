using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly DatabaseService _historyService;

        public HistoryController(DatabaseService historyService)
        {
            _historyService = historyService;
        }
        // GET: api/History
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/History/5
        [HttpGet("{date}", Name = "GetHistoryByDate")]
        public List<History> Get(DateTime date)
        {
            Console.WriteLine(date);
            List<History> myHistories = _historyService.getHistoryByDate(date);
            return myHistories;
        }

        // POST: api/History
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/History/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //////public void Delete(int id)
        //{
        //}
    }
}
