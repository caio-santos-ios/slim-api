using api_slim.src.Models.Base;
using api_slim.src.Responses;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface ITelemedicineService
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync();
        Task<ResponseApi<dynamic?>> UpdateStatusAsync(UpdateStatusTelemedicineDTO request);
    }
}