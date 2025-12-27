using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
    public class UpdateTradingTableDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name {get;set;} = string.Empty; 
        public List<TradingTableItem> Items {get;set;} = []; 
    }
}