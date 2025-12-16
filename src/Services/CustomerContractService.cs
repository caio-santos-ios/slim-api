using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class CustomerContractService(ICustomerContractRepository customerContractRepository, IMapper _mapper) : ICustomerContractService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<CustomerContract> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> customerContracts = await customerContractRepository.GetAllAsync(pagination);
            int count = await customerContractRepository.GetCountDocumentsAsync(pagination);
            return new(customerContracts.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> customerContract = await customerContractRepository.GetByIdAggregateAsync(id);
            if(customerContract.Data is null) return new(null, 404, "Contrato não encontrado");
            return new(customerContract.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<CustomerContract?>> CreateAsync(CreateCustomerContractDTO request)
    {
        try
        {
            CustomerContract customerContract = _mapper.Map<CustomerContract>(request);

            ResponseApi<long> code = await customerContractRepository.GetNextCodeAsync();
            if(!code.IsSuccess) return new(null, 400, "Falha ao criar Contrato.");
            
            customerContract.Code = (code.Data + 1).ToString().PadLeft(6, '0');
            customerContract.CreatedAt = DateTime.UtcNow;
            customerContract.UpdatedAt = DateTime.UtcNow;   
            customerContract.SaleDate = DateTime.UtcNow;

            ResponseApi<CustomerContract?> response = await customerContractRepository.CreateAsync(customerContract);

            if(response.Data is null) return new(null, 400, "Falha ao criar Contrato.");

            return new(response.Data, 201, "Contrato criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<CustomerContract?>> UpdateAsync(UpdateCustomerContractDTO request)
    {
        try
        {
            ResponseApi<CustomerContract?> customerContractResponse = await customerContractRepository.GetByIdAsync(request.Id);
            if(customerContractResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            CustomerContract customerContract = _mapper.Map<CustomerContract>(request);
            customerContract.UpdatedAt = DateTime.UtcNow;

            ResponseApi<CustomerContract?> response = await customerContractRepository.UpdateAsync(customerContract);
            if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");
            
            return new(response.Data, 201, "Atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<CustomerContract>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<CustomerContract> customerContract = await customerContractRepository.DeleteAsync(id);
            if(!customerContract.IsSuccess) return new(null, 400, customerContract.Message);
            return new(null, 204, "Excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion 
}
}