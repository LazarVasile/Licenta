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

        [BsonElement("date_menu")]
        public DateTime dateMenu { get; set; }

        [BsonElement("product_id")]
        public int productId { get; set; }

    }

    public class CreateMenu
    {
        public List<int> listIds { get; set; }
        public DateTime dateMenu { get; set; }
    }
}
