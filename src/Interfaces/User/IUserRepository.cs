using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IUserRepository
    {
        Task<ResponseApi<api_slim.src.Models.User?>> CreateAsync(api_slim.src.Models.User user);
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<api_slim.src.Models.User> pagination);
        Task<ResponseApi<List<dynamic>>> GetSelectBarberAsync(PaginationUtil<api_slim.src.Models.User> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<api_slim.src.Models.User?>> GetByIdAsync(string id);
        Task<ResponseApi<api_slim.src.Models.User?>> GetByUserNameAsync(string userName);
        Task<ResponseApi<api_slim.src.Models.User?>> GetByEmailAsync(string email);
        Task<ResponseApi<api_slim.src.Models.User?>> GetByPhoneAsync(string phone);
        Task<ResponseApi<api_slim.src.Models.User?>> GetByCodeAccessAsync(string codeAccess);
        Task<int> GetCountDocumentsAsync(PaginationUtil<api_slim.src.Models.User> pagination);
        Task<bool> GetAccessValitedAsync(string codeAccess);
        Task<ResponseApi<api_slim.src.Models.User?>> UpdateCodeAccessAsync(string userId, string codeAccess);
        Task<ResponseApi<api_slim.src.Models.User?>> UpdateAsync(api_slim.src.Models.User request);
        Task<ResponseApi<api_slim.src.Models.User?>> ValidatedAccessAsync(string codeAccess);
        Task<ResponseApi<api_slim.src.Models.User>> DeleteAsync(string id);
    }
}