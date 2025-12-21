using Swashbuckle.AspNetCore.Annotations;

namespace api_slim.src.Shared.DTOs
{
    public class CreatePlanDTO
{
        
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ServiceModuleIds { get; set; } = string.Empty;
        public bool Active {get;set;}
        [SwaggerIgnore]
        public IFormFile? Image { get; set; }
}
}