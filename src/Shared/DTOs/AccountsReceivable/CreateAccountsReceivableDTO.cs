namespace api_slim.src.Shared.DTOs
{
    public class CreateAccountsReceivableDTO
    {
        public string Contract {get;set;} = string.Empty;
        public string Category {get;set;} = string.Empty;
        public string CostCenter {get;set;} = string.Empty;      
        public string PaymentMethod {get;set;} = string.Empty;
    }
}