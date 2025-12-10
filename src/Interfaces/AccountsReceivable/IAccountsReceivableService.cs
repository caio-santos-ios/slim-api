using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.DTOs.AccountsReceivable;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IAccountsReceivableService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<AccountsReceivable?>> CreateAsync(CreateAccountsReceivableDTO user);
        Task<ResponseApi<AccountsReceivable?>> UpdateAsync(UpdateAccountsReceivableDTO request);
        Task<ResponseApi<AccountsReceivable>> DeleteAsync(string id);
    }
}