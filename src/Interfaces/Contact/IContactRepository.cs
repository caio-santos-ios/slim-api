using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface IContactRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Contact> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Contact?>> GetByIdAsync(string id);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Contact> pagination);
    Task<ResponseApi<Contact?>> CreateAsync(Contact billing);
    Task<ResponseApi<Contact?>> UpdateAsync(Contact billing);
    Task<ResponseApi<Contact>> DeleteAsync(string id);
}
}