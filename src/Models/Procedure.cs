using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class Procedure : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("serviceModuleId")]
        public string ServiceModuleId { get; set; } = string.Empty;

        [BsonElement("externalCodes")]
        public List<string> ExternalCodes { get; set; } = new List<string>();

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
    }
}