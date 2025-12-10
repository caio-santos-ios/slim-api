using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using api_slim.src.Enums.User;

namespace api_slim.src.Models
{
    public class User : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("name")]
        public string Name {get;set;} = string.Empty;

        [BsonElement("userName")]
        public string UserName {get;set;} = string.Empty;

        [BsonElement("email")]
        public string Email {get;set;} = string.Empty;

        [BsonElement("password")]
        public string Password {get;set;} = string.Empty;

        [BsonElement("phone")]
        public string Phone {get;set;} = string.Empty;

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)] 
        public RoleEnum Role {get;set;} = RoleEnum.Client;

        [BsonElement("blocked")]
        public bool Blocked {get;set;} = false;
        
        [BsonElement("codeAccess")]
        public string CodeAccess {get;set;} = string.Empty;

        [BsonElement("validatedAccess")]
        public bool ValidatedAccess {get;set;} = false;

        [BsonElement("codeAccessExpiration")]
        public DateTime? CodeAccessExpiration { get; set; }

        [BsonElement("photo")]
        public string Photo {get;set;} = string.Empty;
    }
}