using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class BillingItem : ModelBase
    {
        [BsonElement("item")]
        public string Item { get; set; } = string.Empty; 
        
        [BsonElement("start")]
        public string Start { get; set; } = string.Empty; 

        [BsonElement("end")]
        public string End { get; set; } = string.Empty; 
        
        [BsonElement("deliveryDate")]
        public string DeliveryDate { get; set; } = string.Empty;

        [BsonElement("billingDate")]
        public string BillingDate { get; set; } = string.Empty;
    }
}