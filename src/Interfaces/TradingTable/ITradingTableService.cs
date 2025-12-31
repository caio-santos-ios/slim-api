using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface ITradingTableService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<List<dynamic>>> GetSelectAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByaccreditedNetworkIdAsync(string accreditedNetworkId);
        Task<ResponseApi<TradingTable?>> CreateAsync(CreateTradingTableDTO user);
        Task<ResponseApi<TradingTable?>> UpdateAsync(UpdateTradingTableDTO request);
        Task<ResponseApi<TradingTable>> DeleteAsync(string id);
    }
}