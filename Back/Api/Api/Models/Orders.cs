using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api
{
    public class Orders
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("idUser")]
        public int idUser { get; set; } 

        public string typeUser { get; set; }

        [BsonElement("idProductAndAmount")]
        public IDictionary<String, int> idProductsAndAmounts { get; set;  }

        [BsonElement("code")]
        public int code { get; set; }

        [BsonElement("date")]
        public DateTime date { get; set; }

        [BsonElement("totalPrice")]
        public double totalPrice { get; set; }


    }
}
