using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class CustomerRecipientService(ICustomerRecipientRepository customerRepository, IAddressRepository addressRepository, IMapper _mapper) : ICustomerRecipientService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<CustomerRecipient> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> customers = await customerRepository.GetAllAsync(pagination);
            int count = await customerRepository.GetCountDocumentsAsync(pagination);
            return new(customers.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> customer = await customerRepository.GetByIdAggregateAsync(id);
            if(customer.Data is null) return new(null, 404, "Beneficiário não encontrado");
            return new(customer.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }

    public async Task<ResponseApi<List<dynamic>>> GetSelectAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<CustomerRecipient> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> customerRecipient = await customerRepository.GetAllAsync(pagination);
            return new(customerRecipient.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<CustomerRecipient?>> CreateAsync(CreateCustomerRecipientDTO request)
    {
        try
        {
            CustomerRecipient customer = _mapper.Map<CustomerRecipient>(request);
            
            ResponseApi<CustomerRecipient?> response = await customerRepository.CreateAsync(customer);

            if(response.Data is null) return new(null, 400, "Falha ao criar Beneficiário.");

            Address address = _mapper.Map<Address>(request.Address);
            address.Parent = "customer-recipient";
            address.ParentId = response.Data!.Id;
            ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
            if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Item.");

            return new(response.Data, 201, "Beneficiário criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<CustomerRecipient?>> UpdateAsync(UpdateCustomerRecipientDTO request)
    {
        try
        {
            ResponseApi<CustomerRecipient?> customerResponse = await customerRepository.GetByIdAsync(request.Id);
            if(customerResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            CustomerRecipient customer = _mapper.Map<CustomerRecipient>(request);
            customer.UpdatedAt = DateTime.UtcNow;

            ResponseApi<CustomerRecipient?> response = await customerRepository.UpdateAsync(customer);
            if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");

            if(!string.IsNullOrEmpty(request.Address.Id))
            {            
                ResponseApi<Address?> addressResponse = await addressRepository.UpdateAsync(request.Address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao atualizar.");
            }
            else
            {
                Address address = _mapper.Map<Address>(request.Address);
                address.Parent = "customer-recipient";
                address.ParentId = response.Data!.Id;
                ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Item.");
            };
            
            return new(response.Data, 201, "Atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<CustomerRecipient>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<CustomerRecipient> customer = await customerRepository.DeleteAsync(id);
            if(!customer.IsSuccess) return new(null, 400, customer.Message);
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