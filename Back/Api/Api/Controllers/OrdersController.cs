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
    //[Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseService _codesService;

        public OrdersController(DatabaseService codesService)
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
        public List<Orders> Get()
        {
            return _codesService.GetCodes();
        }

        // GET: api/BuyCodes/5
        [HttpGet("{code}", Name = "GetProductsByCode")]
        public Orders Get(int code)
        {
            Console.WriteLine(code);
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            Console.WriteLine(dNow);
            Console.WriteLine(dNow);
            Orders myCode = _codesService.GetCodesByCodeAndDate(code, dNow);
            return myCode; 
        }

        // POST: api/BuyCodes
        [HttpPost]
        public IDictionary<String, String> Post([FromBody] IDictionary<String, double> request)
        {

            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            IMongoCollection<Orders> collection = _codesService.GetCollectionCodes();
            IDictionary<String, String> dict = new Dictionary<String, String>();
            List<Orders> codes = _codesService.GetCodes();
            int code = RandomCode();
            int count = codes.Count;
            while (codes.Exists(x => x.code == code) == true)
            {
                code = RandomCode();
            }

            int idUSer = Convert.ToInt32(request["id_user"]);
            double totalPrice = request["total_price"];
            Orders product = new Orders();
            Console.WriteLine(count);
            product._id = count + 1;
            product.idUser = idUSer;
            product.date = dNow;
            product.code = code;
            product.totalPrice = totalPrice;
            product.idProductsAndAmounts = new Dictionary<String, int>{ };
            User user = _codesService.GetUserById(Convert.ToInt32(request["id_user"]));
            product.typeUser = user.type;
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

    }
}
