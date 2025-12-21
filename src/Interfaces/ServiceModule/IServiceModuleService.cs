using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
public interface IServiceModuleService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<ServiceModule?>> CreateAsync(CreateServiceModuleDTO request);
    Task<ResponseApi<ServiceModule?>> UpdateAsync(UpdateServiceModuleDTO request);
    Task<ResponseApi<ServiceModule?>> UpdateImageAsync(UpdateServiceModuleDTO request);
    Task<ResponseApi<ServiceModule>> DeleteAsync(string id);
}
}