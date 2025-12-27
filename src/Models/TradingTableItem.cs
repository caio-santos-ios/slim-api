using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class TradingTableItem
    {        
        [BsonElement("item")]
        public string Item {get;set;} = string.Empty; 

        [BsonElement("serviceModuleId")]
        public string ServiceModuleId {get;set;} = string.Empty; 

        [BsonElement("procedureId")]
        public string ProcedureId {get;set;} = string.Empty; 
        
        [BsonElement("subTotal")]
        public decimal SubTotal { get; set; }

        [BsonElement("total")]
        public decimal Total { get; set; }

        [BsonElement("discount")]
        public decimal Discount { get; set; }
        
        [BsonElement("discountPercentage")]
        public decimal DiscountPercentage { get; set; }
    }
}