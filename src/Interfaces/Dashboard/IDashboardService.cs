using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface IDashboardService
    {
        Task<ResponseApi<dynamic?>> GetFirstCardAsync();
        Task<ResponseApi<List<dynamic>>> GetRecentPatientAsync();

    }
}