using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface IProcedureService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Procedure?>> CreateAsync(CreateProcedureDTO request);
    Task<ResponseApi<Procedure?>> UpdateAsync(UpdateProcedureDTO request);
    Task<ResponseApi<Procedure>> DeleteAsync(string id);
}
}