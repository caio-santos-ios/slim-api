namespace api_slim.src.Shared.DTOs
{
public class UpdateBillingDTO
{
    public string Id { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Start { get; set; } = string.Empty; 

        public string End { get; set; } = string.Empty; 
        
        public string DeliveryDate { get; set; } = string.Empty;

        public string BillingDateValue { get; set; } = string.Empty;
}
}