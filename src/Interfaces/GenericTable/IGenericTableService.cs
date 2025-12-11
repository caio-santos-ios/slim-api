using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface IGenericTableService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<GenericTable?>> CreateAsync(CreateGenericTableDTO user);
        Task<ResponseApi<GenericTable?>> UpdateAsync(UpdateGenericTableDTO request);
        Task<ResponseApi<GenericTable>> DeleteAsync(string id);
    }
}