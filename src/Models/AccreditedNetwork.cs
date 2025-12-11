using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class AccreditedNetwork : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("cnpj")]
        public string CNPJ { get; set; } = string.Empty;
        
        [BsonElement("tradeName")]
        public string tradeName { get; set; } = string.Empty;

        [BsonElement("corporateName")]
        public string CorporateName { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("whatsapp")]
        public string Whatsapp { get; set; } = string.Empty;
       
        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
       
        [BsonElement("serviceModuleId")]
        public string ServiceModuleId { get; set; } = string.Empty;
        
        [BsonElement("effectiveDate")]
        public DateTime EffectiveDate { get; set; }
        
        [BsonElement("consumptionLimit")]
        public decimal ConsumptionLimit { get; set; }
        
        [BsonElement("tradingYable")]
        public TradingYable TradingYable { get; set; } = new();
    }
    
    public class TradingYable 
    {
        [BsonElement("procedureId")]
        public string ProcedureId { get; set; } = string.Empty;

        [BsonElement("value")]
        public decimal Value { get; set; }
        
        [BsonElement("discountPercentage")]
        public decimal DiscountPercentage { get; set; }
        
        [BsonElement("discountValue")]
        public decimal DiscountValue { get; set; }
      
        [BsonElement("finalValue")]
        public decimal FinalValue { get; set; }
    }
}