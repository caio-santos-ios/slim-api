namespace api_slim.src.Shared.DTOs
{
    public class CreateAccountsReceivableDTO
    {
        public string CustomerId {get;set;} = string.Empty;
        public string ContractId {get;set;} = string.Empty;
        public decimal Value {get;set;} = 0;
        public decimal LowValue {get;set;} = 0;
        public decimal Fines {get;set;} = 0;
        public decimal Fees {get;set;} = 0;
        public DateTime? LowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? BillingDate {get;set;}
    }
}