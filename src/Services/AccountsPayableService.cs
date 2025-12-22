using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class AccountsPayableService(IAccountsPayableRepository accountsPayableRepository, IMapper _mapper) : IAccountsPayableService
    {
        #region READ
        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<AccountsPayable> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> accountsPayables = await accountsPayableRepository.GetAllAsync(pagination);
                int count = await accountsPayableRepository.GetCountDocumentsAsync(pagination);
                return new(accountsPayables.Data, count, pagination.PageNumber, pagination.PageSize);
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
                ResponseApi<dynamic?> accountsPayable = await accountsPayableRepository.GetByIdAggregateAsync(id);
                if(accountsPayable.Data is null) return new(null, accountsPayable.StatusCode, accountsPayable.Message);
                return new(accountsPayable.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        
        #endregion
        #region CREATE
        public async Task<ResponseApi<AccountsPayable?>> CreateAsync(CreateAccountsPayableDTO request)
        {
            try
            {
                DateTime? dueDate = request.DueDate;

                for (int i = 0; i < request.QuantityOfPeriodicity; i++)
                {
                    
                    ResponseApi<long> code = await accountsPayableRepository.GetNextCodeAsync();
                    if(!code.IsSuccess) return new(null, 400, "Falha ao criar Contas a Pagar.");

                    AccountsPayable accountsPayable = _mapper.Map<AccountsPayable>(request);
                    accountsPayable.Code = (code.Data + 1).ToString().PadLeft(6, '0');

                    DateTime? currentBilling = NextBillingDate(request.BillingPeriod, dueDate, request.Billing);

                    accountsPayable.BillingDate = currentBilling;
                    accountsPayable.DueDate = dueDate;

                    ResponseApi<AccountsPayable?> response = await accountsPayableRepository.CreateAsync(accountsPayable);

                    if(response.Data is null) return new(null, 400, "Falha ao criar Conta a Pagar.");

                    if(dueDate is not null) 
                    {
                        dueDate = NextDueDate(request.BillingPeriod, dueDate);
                    }
                };

                return new(null, 201, "Contas a Pagar criada com sucesso.");
            }
            catch
            {                
                return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
            }
        }
        #endregion
        #region UPDATE
        public async Task<ResponseApi<AccountsPayable?>> UpdateAsync(UpdateAccountsPayableDTO request)
        {
            try
            {
                ResponseApi<AccountsPayable?> accountsPayable = await accountsPayableRepository.GetByIdAsync(request.Id);
                if(accountsPayable.Data is null) return new(null, 404, "Falha ao atualizar");

                accountsPayable.Data = _mapper.Map<AccountsPayable>(request);
                accountsPayable.Data.UpdatedAt = DateTime.UtcNow;
                accountsPayable.Data.CreatedAt = accountsPayable.Data.CreatedAt;

                ResponseApi<AccountsPayable?> response = await accountsPayableRepository.UpdateAsync(accountsPayable.Data);
                if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");
                return new(response.Data, 201, "Atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        public async Task<ResponseApi<AccountsPayable?>> UpdateLowAsync(UpdateAccountsPayableDTO request)
        {
            try
            {
                ResponseApi<AccountsPayable?> accountsPayable = await accountsPayableRepository.GetByIdAsync(request.Id);
                if(accountsPayable.Data is null) return new(null, 404, "Falha ao dar baixa");

                accountsPayable.Data.LowDate = DateTime.UtcNow;
                accountsPayable.Data.CreatedAt = accountsPayable.Data.CreatedAt;
                accountsPayable.Data.LowValue += request.LowValue;
                accountsPayable.Data.Fines += request.Fines;
                accountsPayable.Data.Fees += request.Fees;

                ResponseApi<AccountsPayable?> response = await accountsPayableRepository.UpdateLowAsync(accountsPayable.Data);
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
        public async Task<ResponseApi<AccountsPayable>> DeleteAsync(string accountsPayableId)
        {
            try
            {
                ResponseApi<AccountsPayable> accountsPayable = await accountsPayableRepository.DeleteAsync(accountsPayableId);
                if(!accountsPayable.IsSuccess) return new(null, 400, accountsPayable.Message);
                return accountsPayable;
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion       
        #region FUNCTION
        public static DateTime? NextDueDate(string billingPeriod, DateTime? currentDueDate)
        {
            if (currentDueDate == null) return null;

            return billingPeriod switch
            {
                "Mensal" => currentDueDate.Value.AddMonths(1),
                "Semanal" => currentDueDate.Value.AddDays(7),
                "Anual" => currentDueDate.Value.AddYears(1),
                _ => currentDueDate
            };
        }
        public static DateTime? NextBillingDate(string billingPeriod, DateTime? currentDueDate, string billingDay)
        {
            if (currentDueDate == null || string.IsNullOrEmpty(billingDay)) return currentDueDate;

            switch (billingPeriod)
            {
                case "Mensal":
                    int day = Convert.ToInt32(billingDay);
                    int lastDayOfMonth = DateTime.DaysInMonth(currentDueDate.Value.Year, currentDueDate.Value.Month);
                    int safeDay = Math.Min(day, lastDayOfMonth);
                    
                    return new DateTime(currentDueDate.Value.Year, currentDueDate.Value.Month, safeDay);

                case "Anual":
                    var spl = billingDay.Split("-");
                    int lastDayOfAnualMonth = DateTime.DaysInMonth(Convert.ToInt32(spl[0]), Convert.ToInt32(spl[1]));
                    return new DateTime(currentDueDate.Value.Year, currentDueDate.Value.Month, Math.Min(Convert.ToInt32(spl[2]), lastDayOfAnualMonth));

                case "Semanal":
                    return currentDueDate;

                default:
                    return currentDueDate;
            }
        }
        #endregion 
    }
}