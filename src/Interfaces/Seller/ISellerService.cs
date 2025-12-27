using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
public interface ISellerService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<List<dynamic>>> GetSelectAsync(GetAllDTO request);
    Task<ResponseApi<Seller?>> CreateAsync(CreateSellerDTO request);
    Task<ResponseApi<Seller?>> UpdateAsync(UpdateSellerDTO request);
    Task<ResponseApi<Seller>> DeleteAsync(string id);
}
}