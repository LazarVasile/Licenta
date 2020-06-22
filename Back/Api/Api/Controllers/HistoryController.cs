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
    [Route("api/orders/history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly DatabaseService _historyService;

        public HistoryController(DatabaseService historyService)
        {
            _historyService = historyService;
        }

        // GET: api/History/5
        [HttpGet("{date}", Name = "GetHistoryByDate")]
        public List<History> Get(DateTime date)
        {
            Console.WriteLine(date);
            List<History> myHistories = _historyService.getHistoryByDate(date);
            return myHistories;
        }
    }
}
