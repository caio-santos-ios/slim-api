namespace api_slim.src.Shared.DTOs
{
public class CreateBillingItemDTO
{
        public string Item { get; set; } = string.Empty; 
        public string Start { get; set; } = string.Empty; 

        public string End { get; set; } = string.Empty; 
        
        public string DeliveryDate { get; set; } = string.Empty;

        public string BillingDate { get; set; } = string.Empty;
}
}