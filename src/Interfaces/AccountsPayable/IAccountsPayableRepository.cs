using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IAccountsPayableRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<AccountsPayable> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<AccountsPayable?>> GetByIdAsync(string id);
        Task<ResponseApi<long>> GetNextCodeAsync();
        Task<int> GetCountDocumentsAsync(PaginationUtil<AccountsPayable> pagination);
        Task<ResponseApi<AccountsPayable?>> CreateAsync(AccountsPayable user);
        Task<ResponseApi<AccountsPayable?>> UpdateAsync(AccountsPayable request);
        Task<ResponseApi<AccountsPayable?>> UpdateLowAsync(AccountsPayable request);
        Task<ResponseApi<AccountsPayable>> DeleteAsync(string id);
    }
}