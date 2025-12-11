namespace api_slim.src.Shared.DTOs
{
public class UpdateServiceModuleDTO
{
    public string Id { get; set; } = string.Empty;
        public string Code {get;set;} = string.Empty;

        public string Name {get;set;} = string.Empty;

        public string Description {get;set;} = string.Empty;

        public decimal Cost {get;set;}
}
}