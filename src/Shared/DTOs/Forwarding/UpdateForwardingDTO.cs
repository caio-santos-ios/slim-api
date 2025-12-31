namespace api_slim.src.Shared.DTOs
{
    public class UpdateForwardingDTO
    {
        public string Id { get; set; } = string.Empty;
        public string AvailabilityUuid {get;set;} = string.Empty;
        public string BeneficiaryUuid {get;set;} = string.Empty;
        public string SpecialtyUuid {get;set;} = string.Empty;
        public bool ApproveAdditionalPayment {get;set;}
    }
}