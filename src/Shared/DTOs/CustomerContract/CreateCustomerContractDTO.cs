namespace api_slim.src.Shared.DTOs
{
public class CreateCustomerContractDTO
{
        public string Type { get; set; } = string.Empty;
        public string ContractorId { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty;
        public DateTime? SaleDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
        public string ServiceModuleId { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string ReceiptAccount { get; set; } = string.Empty;
        public string PaymentCondition { get; set; } = string.Empty;
        public int PaymentInstallmentQuantity { get; set; } = 0;
        public string DueDateInstallment { get; set; } = string.Empty;
        public string ChargingMethodInstallment { get; set; } = string.Empty;
        public string TypeRecurrence { get; set; } = string.Empty;
        public string Recurrence { get; set; } = string.Empty;
        public DateTime? RecurrencePeriod { get; set; }
        public DateTime? DueDate { get; set; }
        public string BillingPeriod { get; set; } = string.Empty;
        public string Billing { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
}
}