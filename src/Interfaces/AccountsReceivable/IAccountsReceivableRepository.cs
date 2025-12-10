using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;

namespace api_slim.src.Interfaces
{
    public interface IAccountsReceivableRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<AccountsReceivable> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<AccountsReceivable?>> GetByIdAsync(string id);
        Task<int> GetCountDocumentsAsync(PaginationUtil<AccountsReceivable> pagination);
        Task<ResponseApi<AccountsReceivable?>> CreateAsync(AccountsReceivable user);
        Task<ResponseApi<AccountsReceivable?>> UpdateAsync(AccountsReceivable request);
        Task<ResponseApi<AccountsReceivable>> DeleteAsync(string id);
    }
}