namespace api_slim.src.Shared.DTOs
{
    public class UpdatePlanDTO
{
   public string Id { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string ServiceModuleId { get; set; } = string.Empty;

        public bool Active {get;set;}
}
}