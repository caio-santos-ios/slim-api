using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
public class UpdateAccreditedNetworkDTO
{
public string Id { get; set; } = string.Empty;
        
    public string CNPJ { get; set; } = string.Empty;
    
    public string tradeName { get; set; } = string.Empty;

    public string CorporateName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Whatsapp { get; set; } = string.Empty;
    
    public string Notes { get; set; } = string.Empty;
    
    public string ServiceModuleId { get; set; } = string.Empty;
    
    public DateTime EffectiveDate { get; set; }
    
    public decimal ConsumptionLimit { get; set; }
    
    public TradingYable TradingYable { get; set; } = new();
}
}