using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.DTOs.User;

namespace api_slim.src.Interfaces.User
{
    public interface IUserService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request, string userId);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<List<dynamic>>> GetSelectBarberAsync(GetAllDTO request);
        Task<ResponseApi<api_slim.src.Models.User?>> CreateAsync(CreateUserDTO user);
        Task<ResponseApi<api_slim.src.Models.User?>> UpdateAsync(UpdateUserDTO user);
        Task<ResponseApi<api_slim.src.Models.User?>> SavePhotoProfileAsync(SaveUserPhotoDTO user);
        Task<ResponseApi<api_slim.src.Models.User?>> ResendCodeAccessAsync(UpdateUserDTO user);
        Task<ResponseApi<api_slim.src.Models.User?>> RemovePhotoProfileAsync(string id);
        Task<ResponseApi<api_slim.src.Models.User?>> ValidatedAccessAsync(string codeAccess);
        Task<ResponseApi<api_slim.src.Models.User>> DeleteAsync(string id);
    }
}