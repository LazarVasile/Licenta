using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/codes")]
    [ApiController]
    public class CodesController : ControllerBase
    {
        private readonly DatabaseService _codesService;

        public CodesController(DatabaseService codesService)
        {
            _codesService = codesService;
        }

        public static int RandomCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            string stringCode =  new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int code = Int32.Parse(stringCode);

            return code;
        }

        // GET: api/BuyCodes
        [HttpGet]
        public List<Codes> Get()
        {
            return _codesService.GetCodes();
        }

        // GET: api/BuyCodes/5
        [HttpGet("{code}", Name = "GetProductsByCode")]
        public Codes Get(int code)
        {
            Console.WriteLine(code);
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            Console.WriteLine(dNow);
            Console.WriteLine(dNow);
            Codes myCode = _codesService.GetCodesByCodeAndDate(code, dNow);
            return myCode; 
        }

        // POST: api/BuyCodes
        [HttpPost]
        public IDictionary<String, String> Post([FromBody] IDictionary<String, double> request)
        {

            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            IMongoCollection<Codes> collection = _codesService.GetCollectionCodes();
            IDictionary<String, String> dict = new Dictionary<String, String>();
            List<Codes> codes = _codesService.GetCodes();
            int code = RandomCode();
            int count = codes.Count;
            while (codes.Exists(x => x.code == code) == true)
            {
                code = RandomCode();
            }

            int idUSer = Convert.ToInt32(request["id_user"]);
            double totalPrice = request["total_price"];
            Codes product = new Codes();
            Console.WriteLine(count);
            product._id = count + 1;
            product.idUser = idUSer;
            product.date = dNow;
            product.code = code;
            product.totalPrice = totalPrice;
            product.idProductsAndAmounts = new Dictionary<String, int>{ };
            
            foreach (KeyValuePair<string, double> item in request)
            {
                if (item.Key != "id_user" && item.Key != "total_price")
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                    product.idProductsAndAmounts[item.Key] = Convert.ToInt32(item.Value);
                }
            }
            try
            {
                collection.InsertOneAsync(product);
            }
            catch (Exception)
            {
                Console.WriteLine("nu s-a putut insera");
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
