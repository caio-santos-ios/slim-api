using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
   public interface IBillingService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<List<dynamic>>> GetSelectAsync(GetAllDTO request);
    Task<ResponseApi<Billing?>> CreateAsync(CreateBillingDTO request);
    Task<ResponseApi<Billing?>> UpdateAsync(UpdateBillingDTO request);
    Task<ResponseApi<Billing>> DeleteAsync(string id);
}
}