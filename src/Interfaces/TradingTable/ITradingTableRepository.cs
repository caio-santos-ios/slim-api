using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface ITradingTableRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<TradingTable> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<TradingTable?>> GetByIdAsync(string id);
        Task<ResponseApi<List<dynamic>>> GetSelectAsync(PaginationUtil<TradingTable> pagination);
        Task<int> GetCountDocumentsAsync(PaginationUtil<TradingTable> pagination);
        Task<ResponseApi<TradingTable?>> CreateAsync(TradingTable user);
        Task<ResponseApi<TradingTable?>> UpdateAsync(TradingTable request);
        Task<ResponseApi<TradingTable>> DeleteAsync(string id);
    }
}