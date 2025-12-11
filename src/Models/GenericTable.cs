using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class GenericTable : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("table")]
        public string Table {get;set;} = string.Empty;

        [BsonElement("code")]
        public string Code {get;set;} = string.Empty;

        [BsonElement("description")]
        public string Description {get;set;} = string.Empty;
    }
}