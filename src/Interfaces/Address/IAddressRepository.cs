using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface IAddressRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Address> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Address?>> GetByIdAsync(string id);
    Task<ResponseApi<Address?>> GetByParentIdAsync(string parentId, string parent);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Address> pagination);
    Task<ResponseApi<Address?>> CreateAsync(Address address);
    Task<ResponseApi<Address?>> UpdateAsync(Address address);
    Task<ResponseApi<Address>> DeleteAsync(string id);
}
}