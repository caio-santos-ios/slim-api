using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class AccountsReceivable : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("contract")]
        public string Contract {get;set;} = string.Empty;

        [BsonElement("category")]
        public string Category {get;set;} = string.Empty;

        [BsonElement("costCenter")]
        public string CostCenter {get;set;} = string.Empty;
      
        [BsonElement("paymentMethod")]
        public string PaymentMethod {get;set;} = string.Empty;
    }
}