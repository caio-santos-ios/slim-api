using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class AccreditedNetworkService(IAccreditedNetworkRepository accreditedNetwork, IAddressRepository addressRepository, IMapper _mapper) : IAccreditedNetworkService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<AccreditedNetwork> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> accrediteds = await accreditedNetwork.GetAllAsync(pagination);
            int count = await accreditedNetwork.GetCountDocumentsAsync(pagination);
            return new(accrediteds.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> accredited = await accreditedNetwork.GetByIdAggregateAsync(id);
            if(accredited.Data is null) return new(null, 404, "Rede Credenciada não encontrado");
            return new(accredited.Data);
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
            PaginationUtil<AccreditedNetwork> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> accrediteds = await accreditedNetwork.GetAllAsync(pagination);
            return new(accrediteds.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<AccreditedNetwork?>> CreateAsync(CreateAccreditedNetworkDTO request)
    {
        try
        {
            AccreditedNetwork accredited = _mapper.Map<AccreditedNetwork>(request);
            ResponseApi<AccreditedNetwork?> response = await accreditedNetwork.CreateAsync(accredited);

            if(response.Data is null) return new(null, 400, "Falha ao criar Rede Credenciada.");

            if(!string.IsNullOrEmpty(request.Address.Id))
            {
                ResponseApi<Address?> findAddress = await addressRepository.GetByParentIdAsync(request.Address.ParentId, request.Address.Parent);
                if(!findAddress.IsSuccess || findAddress.Data is null) return new(null, 400, "Falha ao atualizar.");
                
                request.Address.Id = findAddress.Data.Id;
            
                ResponseApi<Address?> addressResponse = await addressRepository.UpdateAsync(request.Address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao atualizar.");
            }
            else
            {
                Address address = _mapper.Map<Address>(request.Address);
                address.ParentId = response.Data.Id;
                ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Rede Credenciada.");
            };

            return new(response.Data, 201, "Rede Credenciada criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<AccreditedNetwork?>> UpdateAsync(UpdateAccreditedNetworkDTO request)
    {
        try
        {
            ResponseApi<AccreditedNetwork?> accreditedResponse = await accreditedNetwork.GetByIdAsync(request.Id);
            if(accreditedResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            AccreditedNetwork accredited = _mapper.Map<AccreditedNetwork>(request);
            accredited.UpdatedAt = DateTime.UtcNow;
            accredited.CreatedAt = accreditedResponse.Data.CreatedAt;

            ResponseApi<AccreditedNetwork?> response = await accreditedNetwork.UpdateAsync(accredited);
            if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");

            if(!string.IsNullOrEmpty(request.Address.Id))
            {
                ResponseApi<Address?> findAddress = await addressRepository.GetByParentIdAsync(request.Address.ParentId, request.Address.Parent);
                if(!findAddress.IsSuccess || findAddress.Data is null) return new(null, 400, "Falha ao atualizar.");
                
                request.Address.Id = findAddress.Data.Id;
            
                ResponseApi<Address?> addressResponse = await addressRepository.UpdateAsync(request.Address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao atualizar.");
            }
            else
            {
                Address address = _mapper.Map<Address>(request.Address);
                ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Item.");
            };

            if(!string.IsNullOrEmpty(request.Responsible.Address.Id))
            {
                ResponseApi<Address?> findAddress = await addressRepository.GetByParentIdAsync(request.Responsible.Address.ParentId, request.Responsible.Address.Parent);
                if(!findAddress.IsSuccess || findAddress.Data is null) return new(null, 400, "Falha ao atualizar.");
                
                request.Responsible.Address.Id = findAddress.Data.Id;
            
                ResponseApi<Address?> addressResponse = await addressRepository.UpdateAsync(request.Responsible.Address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao atualizar.");
            }
            else
            {
                Address address = _mapper.Map<Address>(request.Responsible.Address);
                ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
                if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Rede Credenciada.");
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
    public async Task<ResponseApi<AccreditedNetwork>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<AccreditedNetwork> accredited = await accreditedNetwork.DeleteAsync(id);
            if(!accredited.IsSuccess) return new(null, 400, accredited.Message);
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