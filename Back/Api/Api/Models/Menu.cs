using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace Api
{
    public class Menu
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("dateMenu")]
        public DateTime dateMenu { get; set; }

        [BsonElement("productsIdAndAmounts")]
        public IDictionary<string, int> productsIdAndAmounts {get; set;}

        
    }

    public class CreateMenu
    {
        public Dictionary<string, int> products { get; set; }
        public DateTime dateMenu { get; set; }
    }
}
