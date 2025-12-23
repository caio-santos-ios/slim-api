using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class InPerson : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("recipientId")]
        public string RecipientId {get;set;} = string.Empty; 
        
        [BsonElement("accreditedNetworkId")]
        public string AccreditedNetworkId {get;set;} = string.Empty; 
        
        [BsonElement("serviceModuleId")]
        public string ServiceModuleId {get;set;} = string.Empty; 

        [BsonElement("procedureId")]
        public string ProcedureId {get;set;} = string.Empty; 

        [BsonElement("date")]
        public DateTime? Date { get; set; }
       
        [BsonElement("hour")]
        public string Hour { get; set; } = string.Empty;
      
        [BsonElement("responsiblePayment")]
        public string ResponsiblePayment { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;
    }
}