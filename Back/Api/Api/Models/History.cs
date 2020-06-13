using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace Api
{
    public class History
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("date")]
        public DateTime date { get; set; }

        public IDictionary<string, int> nameProductsAndAmounts { get; set; }

        public double totalPrice { get; set; }
    }

}
