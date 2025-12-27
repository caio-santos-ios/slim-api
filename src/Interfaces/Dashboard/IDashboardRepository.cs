using api_slim.src.Models.Base;

namespace api_slim.src.Interfaces
{
    public interface IDashboardRepository
    {
        Task<ResponseApi<dynamic>> GetFirstCardAsync();
    }
}