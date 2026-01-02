using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IHistoricRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Historic> pagination);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Historic> pagination);
        Task<ResponseApi<Historic?>> CreateAsync(Historic historic);
    }
}