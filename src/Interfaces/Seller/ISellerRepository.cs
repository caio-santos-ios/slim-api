using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface ISellerRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Seller> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Seller?>> GetByIdAsync(string id);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Seller> pagination);
    Task<ResponseApi<Seller?>> CreateAsync(Seller seller);
    Task<ResponseApi<Seller?>> UpdateAsync(Seller seller);
    Task<ResponseApi<Seller>> DeleteAsync(string id);
}
}