using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface ISupplierRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Supplier> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Supplier?>> GetByIdAsync(string id);
    Task<ResponseApi<List<dynamic>>> GetSelectAsync(PaginationUtil<Supplier> pagination);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Supplier> pagination);
    Task<ResponseApi<Supplier?>> CreateAsync(Supplier address);
    Task<ResponseApi<Supplier?>> UpdateAsync(Supplier address);
    Task<ResponseApi<Supplier>> DeleteAsync(string id);
}
}