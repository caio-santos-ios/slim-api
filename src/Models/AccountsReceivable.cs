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
        
        [BsonElement("customerId")]
        public string CustomerId {get;set;} = string.Empty;

        [BsonElement("contractId")]
        public string ContractId {get;set;} = string.Empty;
        
        [BsonElement("value")]
        public decimal Value {get;set;} = 0;

        [BsonElement("lowValue")]
        public decimal LowValue {get;set;} = 0;
        
        [BsonElement("lowDate")]
        public DateTime? LowDate { get; set; }
    }
}