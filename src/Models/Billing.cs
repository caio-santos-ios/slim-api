using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class Billing : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("start")]
        public string Start { get; set; } = string.Empty; 

        [BsonElement("end")]
        public string End { get; set; } = string.Empty; 
        
        [BsonElement("deliveryDate")]
        public string DeliveryDate { get; set; } = string.Empty;

        [BsonElement("billingDate")]
        public string BillingDateValue { get; set; } = string.Empty;
    }
}