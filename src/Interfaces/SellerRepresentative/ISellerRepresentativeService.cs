using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface ISellerRepresentativeService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<SellerRepresentative?>> CreateAsync(CreateSellerRepresentativeDTO request);
        Task<ResponseApi<SellerRepresentative?>> UpdateAsync(UpdateSellerRepresentativeDTO request);
        Task<ResponseApi<SellerRepresentative>> DeleteAsync(string id);
    }
}