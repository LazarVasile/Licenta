using MongoDB.Bson.Serialization.Attributes;

namespace Api
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public int _id { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

        [BsonElement("role")]
        public string role { get; set; }

        [BsonElement("type")]
        public string type { get; set; }
    }
}
