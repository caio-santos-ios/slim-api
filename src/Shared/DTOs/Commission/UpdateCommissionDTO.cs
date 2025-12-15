using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
public class UpdateCommissionDTO
{
        public string Id { get; set; } = string.Empty;
        
        public string RuleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string SaleType { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
         public string TypeModality { get; set; } = string.Empty;
        public decimal ValueModality { get; set; }
        public decimal PercentageModality { get; set; }
        public int StartNumberModality { get; set; }
        public int EndNumberModality { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public string Liquidation { get; set; } = string.Empty;
        public string ReferenceCriteria { get; set; } = string.Empty;
        public string PaymentDate { get; set; } = string.Empty;
        public Campaign Campaign { get; set; } = new();
        public bool IsAgency { get; set; } = false;
        public Agency Agency { get; set; } = new();
        public bool IsRecurrence { get; set; } = false;
        public Recurrence Recurrence { get; set; } = new();
}
}