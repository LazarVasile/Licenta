using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/codes")]
    [ApiController]
    public class BuyCodesController : ControllerBase
    {
        private readonly DatabaseService _codesService;

        public BuyCodesController(DatabaseService codesService)
        {
            _codesService = codesService;
        }

        private static Random random = new Random();
        public static int RandomCode()
        {
            const string chars = "0123456789";
            string stringCode =  new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int code = Int32.Parse(stringCode);

            return code;
        }

        // GET: api/BuyCodes
        [HttpGet]
        public List<BuyProducts> Get()
        {
            return _codesService.GetCodes();
        }

        // GET: api/BuyCodes/5
        [HttpGet("{code}", Name = "Get")]
        public List<BuyProducts> Get(int code)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
            Console.WriteLine(dNow);
            List<BuyProducts> myCode = _codesService.GetCodesByCodeAndDate(code, dNow);
            return myCode;

            
            
        }

        // POST: api/BuyCodes
        [HttpPost]
        public IDictionary<String, String> Post([FromBody] IDictionary<String, int> request)
        {
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));

            IMongoCollection<BuyProducts> collection = _codesService.GetCollectionCodes();
            IDictionary<String, String> dict = new Dictionary<String, String>();
            int count = _codesService.GetCodes().Count;
            List<BuyProducts> codes = _codesService.GetCodesByDate(dNow);
            int code = RandomCode();

            while (codes.Exists(x => x.code == code) == true)
            {
                code = RandomCode();
            }

            foreach (KeyValuePair<String, int> item in request)
            {
               
                Console.WriteLine(item.Key + ":" + item.Value);
                count += 1;
                BuyProducts product = new BuyProducts();
                product._id = count;
                product.idProduct = Int32.Parse(item.Key);
                product.quantity = item.Value;
                product.date = dNow;
                product.code = code;
                try
                {
                    collection.InsertOneAsync(product);
                }
                catch (Exception) { Console.WriteLine("Nu s-a putut adauga in baza de date"); }
            }
            dict.Add("response", "true");
            dict.Add("code", code.ToString());

            return dict;
        }

        // PUT: api/BuyCodes/5
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
