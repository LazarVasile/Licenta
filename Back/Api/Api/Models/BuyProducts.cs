using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Api
{
    public class BuyProducts
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("idProduct")]
        public int idProduct { get; set;  }

        [BsonElement("code")]
        public int code { get; set; }

        [BsonElement("date")]
        public DateTime date { get; set; }

        [BsonElement("quantity")]
        public int quantity { get; set; }

    }
}
