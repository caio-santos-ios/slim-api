using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class AccountsReceivableService(IAccountsReceivableRepository accountsReceivableRepository, IMapper _mapper) : IAccountsReceivableService
    {
        #region READ
        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<AccountsReceivable> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> accountsReceivables = await accountsReceivableRepository.GetAllAsync(pagination);
                int count = await accountsReceivableRepository.GetCountDocumentsAsync(pagination);
                return new(accountsReceivables.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        public async Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id)
        {
            try
            {
                ResponseApi<dynamic?> accountsReceivable = await accountsReceivableRepository.GetByIdAggregateAsync(id);
                if(accountsReceivable.Data is null) return new(null, 404, "Conta a receber não encontrada");
                return new(accountsReceivable.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        public async Task<ResponseApi<List<AccountsReceivable>>> GetByContractId(string contractId)
        {
            try
            {
                ResponseApi<List<AccountsReceivable>> accountsReceivable = await accountsReceivableRepository.GetByContractId(contractId);
                if(accountsReceivable.Data is null) return new(null, 404, "Conta a receber não encontrada");
                return new(accountsReceivable.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        #region CREATE
        public async Task<ResponseApi<AccountsReceivable?>> CreateAsync(CreateAccountsReceivableDTO request)
        {
            try
            {
                
                ResponseApi<long> code = await accountsReceivableRepository.GetNextCodeAsync();
                if(!code.IsSuccess) return new(null, 400, "Falha ao criar Contas a Receber.");

                AccountsReceivable accountsReceivable = _mapper.Map<AccountsReceivable>(request);
                accountsReceivable.Code = (code.Data + 1).ToString().PadLeft(6, '0');
                ResponseApi<AccountsReceivable?> response = await accountsReceivableRepository.CreateAsync(accountsReceivable);

                if(response.Data is null) return new(null, 400, "Falha ao criar conta.");
                return new(response.Data, 201, "Contas a receber criada com sucesso.");
            }
            catch
            {                
                return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
            }
        }
        #endregion
        #region UPDATE
        public async Task<ResponseApi<AccountsReceivable?>> UpdateAsync(UpdateAccountsReceivableDTO request)
        {
            try
            {
                ResponseApi<AccountsReceivable?> accountsReceivable = await accountsReceivableRepository.GetByIdAsync(request.Id);
                if(accountsReceivable.Data is null) return new(null, 404, "Falha ao atualizar");

                accountsReceivable.Data = _mapper.Map<AccountsReceivable>(request);
                accountsReceivable.Data.UpdatedAt = DateTime.UtcNow;
                accountsReceivable.Data.CreatedAt = accountsReceivable.Data.CreatedAt;

                ResponseApi<AccountsReceivable?> response = await accountsReceivableRepository.UpdateAsync(accountsReceivable.Data);
                if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");
                return new(response.Data, 201, "Atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        public async Task<ResponseApi<AccountsReceivable?>> UpdateLowAsync(UpdateAccountsReceivableDTO request)
        {
            try
            {
                ResponseApi<AccountsReceivable?> accountsReceivable = await accountsReceivableRepository.GetByIdAsync(request.Id);
                if(accountsReceivable.Data is null) return new(null, 404, "Falha ao dar baixa");

                accountsReceivable.Data.LowDate = DateTime.UtcNow;
                accountsReceivable.Data.CreatedAt = accountsReceivable.Data.CreatedAt;
                accountsReceivable.Data.LowValue += request.LowValue;
                accountsReceivable.Data.Fines += request.Fines;
                accountsReceivable.Data.Fees += request.Fees;

                ResponseApi<AccountsReceivable?> response = await accountsReceivableRepository.UpdateLowAsync(accountsReceivable.Data);
                if(!response.IsSuccess) return new(null, 400, "Falha ao dar baixa");
                return new(response.Data, 201, "Baixa feita com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        
        #region DELETE
        public async Task<ResponseApi<AccountsReceivable>> DeleteAsync(string accountsReceivableId)
        {
            try
            {
                ResponseApi<AccountsReceivable> accountsReceivable = await accountsReceivableRepository.DeleteAsync(accountsReceivableId);
                if(!accountsReceivable.IsSuccess) return new(null, 400, accountsReceivable.Message);
                return accountsReceivable;
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion        
    }
}