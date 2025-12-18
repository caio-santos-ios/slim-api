namespace api_slim.src.Shared.DTOs
{
    public class UpdateAccountsReceivableDTO
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId {get;set;} = string.Empty;
        public string ContractId {get;set;} = string.Empty;
        public decimal Value {get;set;} = 0;
        public decimal LowValue {get;set;} = 0;
        public DateTime? LowDate { get; set; }
    }
}