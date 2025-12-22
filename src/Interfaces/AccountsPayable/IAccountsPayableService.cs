using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Interfaces
{
    public interface IAccountsPayableService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<AccountsPayable?>> CreateAsync(CreateAccountsPayableDTO user);
        Task<ResponseApi<AccountsPayable?>> UpdateAsync(UpdateAccountsPayableDTO request);
        Task<ResponseApi<AccountsPayable?>> UpdateLowAsync(UpdateAccountsPayableDTO request);
        Task<ResponseApi<AccountsPayable>> DeleteAsync(string id);
    }
}