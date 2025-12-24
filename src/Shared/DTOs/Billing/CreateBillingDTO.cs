namespace api_slim.src.Shared.DTOs
{
public class CreateBillingDTO
{
      public string Id { get; set; } = string.Empty;
        
      public string Name { get; set; } = string.Empty;

      public string Description { get; set; } = string.Empty;

      public List<CreateBillingItemDTO> Items { get; set; } = []; 
}
}