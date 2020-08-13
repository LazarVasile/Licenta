using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/products/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public readonly DatabaseService _menuService;
        private Font printFont;
        private StreamReader streamToPrint;
        static string filePath;


        public MenuController (DatabaseService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menu
        [HttpGet]
        public ActionResult<List<Menu>> GetMenus() =>
            _menuService.GetMenus();


        //GET: api/Menu/5
        [HttpGet("{date}", Name = "GetMenusByDate")]
        public Menu GetMenuByDate(DateTime date)
        {
            Menu myMenu = _menuService.GetLastMenuByDate(date);
            return myMenu;

        }


        [HttpPost]
        public IDictionary<string, string> Post([FromBody] CreateMenu request)
        {
            List<Menu> menus = _menuService.GetMenus();
            IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
            //Console.WriteLine(value.dateMenu);
            var number = menus.Count;
            DateTime myDate = Convert.ToDateTime(request.dateMenu.ToString("yyyy-MM-dd"));
            var newMenu = new Menu();
            newMenu._id = number + 1;
            newMenu.dateMenu = myDate;
            newMenu.productsIdAndAmounts = new Dictionary<String, int> { };
            var dict = new Dictionary<string, string> { };
            foreach(KeyValuePair<string, int> item in request.products)
            {
                newMenu.productsIdAndAmounts[item.Key] = item.Value;
            }
            try
            {
                collection.InsertOneAsync(newMenu);
                dict["response"] = "true";
                return dict;
            } catch 
            {
                dict["response"] = "false";
                return dict;
            }
          
            

        }

        public static int RandomCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            string stringCode = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int code = Int32.Parse(stringCode);

            return code;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        // Print the file.
        public void Printing(string filePath)
        {
            try
            {
                streamToPrint = new StreamReader(filePath);
                try
                {
                    printFont = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    // Print the document.

                    
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [HttpPut]
        public History Put([FromBody] IDictionary<String, double> request)
        {
            Console.WriteLine(request["total_price"] +  request["others"]);
            DateTime dNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            IMongoCollection<Menu> collection = _menuService.GetCollectionMenu();
            IMongoCollection<History> collection2 = _menuService.GetCollectionHistory();
            int code = RandomCode();

            List<History> histories = _menuService.getHistories();
            History history = new History();
            while (histories.Exists(x => x._id == code) == true)
            {
                code = RandomCode();
            }
            history._id = code;
            history.date = dNow;
            List<Product> products = _menuService.GetProducts();
            history.nameProductsAndAmounts = new Dictionary<String, int> { };
            history.totalPrice = request["total_price"];
            Menu menu = _menuService.GetLastMenuByDate(dNow);
            IDictionary<string, int> copy =  new Dictionary<string, int>(menu.productsIdAndAmounts);
            List<string> lines = new List<string> ();
            string dateString =DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            lines.Add("                        Cantină\n");
            lines.Add("Cod:" + Convert.ToString(code));
            lines.Add("--------------------------------------------------");

            foreach (KeyValuePair<string, double> item in request)
            {
                if (item.Key != "total_price" && item.Key != "others")
                {
                    try
                    {
                        var product = products.Find(p => p._id == Convert.ToInt32(item.Key));
                        history.nameProductsAndAmounts[product.name] = Convert.ToInt32(item.Value);
                        copy[item.Key] -= Convert.ToInt32(item.Value);
                        Console.WriteLine(request["others"]);
                        if(request["others"] == 1)
                        {
                            lines.Add(product.name);
                            var my_string = product.professorPrice + "X" + item.Value + " = " + (Convert.ToInt32(item.Value) * product.professorPrice).ToString();
                            lines.Add(my_string);
                        }
                        else
                        {
                            lines.Add(product.name);
                            var my_string = product.studentPrice + "X" + item.Value + " = " + (Convert.ToInt32(item.Value) * product.studentPrice).ToString();
                            lines.Add(my_string);

                        }
                    }
                    catch
                    {
                        return history;
                    }

                }
            }
            lines.Add("--------------------------------------------------");
            
            var my_string1 = "PREȚ TOTAL: " + request["total_price"].ToString() + " LEI";
            lines.Add(my_string1);
            lines.Add("Data: " + dateString);
            System.IO.File.WriteAllLines("receipt.txt", lines);
            Printing("receipt.txt");
            var arrayFilter = Builders<Menu>.Filter.Eq("dateMenu", dNow) & Builders<Menu>.Filter.Eq("productsIdAndAmounts", menu.productsIdAndAmounts);
            var update = Builders<Menu>.Update.Set("productsIdAndAmounts", copy);

            try
            {
                collection.UpdateOne(arrayFilter, update);
            }
            catch (Exception)
            {
                Console.WriteLine("Nu se poate face update");
            }

            collection2.InsertOneAsync(history);
            return history;

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
