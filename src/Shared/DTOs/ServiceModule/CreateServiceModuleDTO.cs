using Swashbuckle.AspNetCore.Annotations;

namespace api_slim.src.Shared.DTOs
{
public class CreateServiceModuleDTO
{        
        public string Code {get;set;} = string.Empty;
        public string Name {get;set;} = string.Empty;
        public string Description {get;set;} = string.Empty;
        public string Type {get;set;} = string.Empty;
        public decimal Price {get;set;}
        public decimal Cost {get;set;}
        public bool Active {get;set;}
        [SwaggerIgnore]
        public IFormFile? Image { get; set; }
}
}