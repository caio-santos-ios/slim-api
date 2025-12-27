using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface IBillingRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Billing> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Billing?>> GetByIdAsync(string id);
    Task<ResponseApi<List<dynamic>>> GetSelectAsync(PaginationUtil<Billing> pagination);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Billing> pagination);
    Task<ResponseApi<Billing?>> CreateAsync(Billing billing);
    Task<ResponseApi<Billing?>> UpdateAsync(Billing billing);
    Task<ResponseApi<Billing>> DeleteAsync(string id);
}
}