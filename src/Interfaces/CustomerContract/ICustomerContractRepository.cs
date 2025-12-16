using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface ICustomerContractRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<CustomerContract> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<CustomerContract?>> GetByIdAsync(string id);
    Task<ResponseApi<long>> GetNextCodeAsync();
    Task<int> GetCountDocumentsAsync(PaginationUtil<CustomerContract> pagination);
    Task<ResponseApi<CustomerContract?>> CreateAsync(CustomerContract address);
    Task<ResponseApi<CustomerContract?>> UpdateAsync(CustomerContract address);
    Task<ResponseApi<CustomerContract>> DeleteAsync(string id);
}
}