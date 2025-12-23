namespace api_slim.src.Shared.DTOs
{
    public class UpdateInPersonDTO
    {
        public string Id { get; set; } = string.Empty;
        public string RecipientId {get;set;} = string.Empty; 
        public string AccreditedNetworkId {get;set;} = string.Empty; 
        public string ServiceModuleId {get;set;} = string.Empty;
        public string ProcedureId {get;set;} = string.Empty;
        public DateTime? Date { get; set; }
        public string Hour { get; set; } = string.Empty;
        public string ResponsiblePayment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}