namespace api_slim.src.Shared.DTOs
{
    public class UpdateProcedureDTO
{
public string Id { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ServiceModuleId { get; set; } = string.Empty;

        public List<string> ExternalCodes { get; set; } = new List<string>();

        public string Notes { get; set; } = string.Empty;
}
}