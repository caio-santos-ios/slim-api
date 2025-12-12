using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
   public interface IAddressService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<Address?>> CreateAsync(CreateAddressDTO request);
    Task<ResponseApi<Address?>> UpdateAsync(UpdateAddressDTO request);
    Task<ResponseApi<Address>> DeleteAsync(string id);
}
}