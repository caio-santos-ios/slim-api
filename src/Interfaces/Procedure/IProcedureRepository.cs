using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IProcedureRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Procedure> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<Procedure?>> GetByIdAsync(string id);
        Task<ResponseApi<long>> GetNextCodeAsync();
        Task<ResponseApi<List<dynamic>>> GetSelectAsync(PaginationUtil<Procedure> pagination);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Procedure> pagination);
        Task<ResponseApi<Procedure?>> CreateAsync(Procedure procedure);
        Task<ResponseApi<Procedure?>> UpdateAsync(Procedure procedure);
        Task<ResponseApi<Procedure>> DeleteAsync(string id);
    }
}