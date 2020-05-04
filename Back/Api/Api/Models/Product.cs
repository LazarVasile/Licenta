using MongoDB.Bson.Serialization.Attributes;


namespace Api
{
    public class Product
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("category")]
        public string category { get; set; }

        [BsonElement("professor_price")]
        public double professor_price { get; set; }

        [BsonElement("student_price")]
        public double student_price { get; set; }

        [BsonElement("weight")]
        public int weight { get; set; }
        
        [BsonElement("description")]
        public string description { get; set; }


    }
}
