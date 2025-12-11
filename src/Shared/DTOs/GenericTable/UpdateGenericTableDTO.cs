namespace api_slim.src.Shared.DTOs
{
    public class UpdateGenericTableDTO
    {
        public string Id { get; set; } = string.Empty;
        
        public string Table {get;set;} = string.Empty;

        public string Code {get;set;} = string.Empty;

        public string Description {get;set;} = string.Empty;
    }
}