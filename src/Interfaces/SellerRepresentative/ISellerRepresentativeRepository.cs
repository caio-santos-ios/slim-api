using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface ISellerRepresentativeRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<SellerRepresentative> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<SellerRepresentative?>> GetByIdAsync(string id);
        Task<int> GetCountDocumentsAsync(PaginationUtil<SellerRepresentative> pagination);
        Task<ResponseApi<SellerRepresentative?>> CreateAsync(SellerRepresentative plan);
        Task<ResponseApi<SellerRepresentative?>> UpdateAsync(SellerRepresentative plan);
        Task<ResponseApi<SellerRepresentative>> DeleteAsync(string id);
    }
}