namespace api_slim.src.Shared.DTOs
{
public class CreateProcedureDTO
{
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ServiceModuleId { get; set; } = string.Empty;

        public string ExternalCodes { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
                public bool Active {get;set;}

}
}