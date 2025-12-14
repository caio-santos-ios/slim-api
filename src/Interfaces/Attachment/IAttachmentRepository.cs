using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
public interface IAttachmentRepository
{
    Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Attachment> pagination);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Attachment?>> GetByIdAsync(string id);
    Task<ResponseApi<Attachment?>> GetByParentIdAsync(string parentId, string parent);
    Task<int> GetCountDocumentsAsync(PaginationUtil<Attachment> pagination);
    Task<ResponseApi<Attachment?>> CreateAsync(Attachment address);
    Task<ResponseApi<Attachment?>> UpdateAsync(Attachment address);
    Task<ResponseApi<Attachment>> DeleteAsync(string id);
}
}