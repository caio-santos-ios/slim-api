namespace api_slim.src.Shared.DTOs
{
    public class UpdateAccountsReceivableDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Contract {get;set;} = string.Empty;
        public string Category {get;set;} = string.Empty;
        public string CostCenter {get;set;} = string.Empty;      
        public string PaymentMethod {get;set;} = string.Empty;
    }
}