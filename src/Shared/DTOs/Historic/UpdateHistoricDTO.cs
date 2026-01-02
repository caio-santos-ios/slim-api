namespace api_slim.src.Shared.DTOs
{
        public class UpdateHistoricDTO
        {
                public string Id { get; set; } = string.Empty;
                public string Collection { get; set; } = string.Empty;
                public string Description { get; set; } = string.Empty;
                public string Recipient { get; set; } = string.Empty;
        }
}