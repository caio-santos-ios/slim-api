using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
   public interface ICustomerRecipientService
{
    Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
    Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
    Task<ResponseApi<CustomerRecipient?>> CreateAsync(CreateCustomerRecipientDTO request);
    Task<ResponseApi<CustomerRecipient?>> UpdateAsync(UpdateCustomerRecipientDTO request);
    Task<ResponseApi<CustomerRecipient>> DeleteAsync(string id);
}
}