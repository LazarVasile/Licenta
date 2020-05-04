using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class AddProduct
    {
        public int _id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public double professor_price { get; set; }
        public double student_price { get; set; }
        public int weight { get; set; }
        public string description { get; set; }
    }
}
