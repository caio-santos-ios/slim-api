namespace api_slim.src.Shared.DTOs
{
    public class UpdateAccountsPayableDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ReferenceCode {get;set;} = string.Empty;
        public string SupplierId {get;set;} = string.Empty;
        public string CostCenter {get;set;} = string.Empty;
        public string Category {get;set;} = string.Empty;
        public string TypeOfPeriodicity {get;set;} = string.Empty;
        public int QuantityOfPeriodicity {get;set;}
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Value {get;set;} = 0;
        public decimal LowValue {get;set;} = 0;
        public decimal Fines {get;set;} = 0;
        public decimal Fees {get;set;} = 0;
        public DateTime? LowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? BillingDate { get; set; }
        public string BillingPeriod { get; set; } = string.Empty;
        public string Billing { get; set; } = string.Empty;
    }
}