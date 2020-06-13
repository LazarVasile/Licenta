using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
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
        [HttpGet("{id}", Name = "GetHistoryByDate")]
        public List<History> Get(DateTime date)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            List<History> myHistories = _historyService.getHistoryByDate(dNow);
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
        [HttpDelete("{id}")]
        //////public void Delete(int id)
        {
        }
    }
}
