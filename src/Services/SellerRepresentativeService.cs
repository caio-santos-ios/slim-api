using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class SellerRepresentativeService(ISellerRepresentativeRepository billingRepository, IMapper _mapper) : ISellerRepresentativeService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<SellerRepresentative> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> billings = await billingRepository.GetAllAsync(pagination);
            int count = await billingRepository.GetCountDocumentsAsync(pagination);
            return new(billings.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> billing = await billingRepository.GetByIdAggregateAsync(id);
            if(billing.Data is null) return new(null, 404, "Item não encontrado");
            return new(billing.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<SellerRepresentative?>> CreateAsync(CreateSellerRepresentativeDTO request)
    {
        try
        {
            SellerRepresentative billing = _mapper.Map<SellerRepresentative>(request);
            ResponseApi<SellerRepresentative?> response = await billingRepository.CreateAsync(billing);

            if(response.Data is null) return new(null, 400, "Falha ao criar Item.");
            return new(response.Data, 201, "Item criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<SellerRepresentative?>> UpdateAsync(UpdateSellerRepresentativeDTO request)
    {
        try
        {
            ResponseApi<SellerRepresentative?> billingResponse = await billingRepository.GetByIdAsync(request.Id);
            if(billingResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            SellerRepresentative billing = _mapper.Map<SellerRepresentative>(request);
            billing.UpdatedAt = DateTime.UtcNow;

            ResponseApi<SellerRepresentative?> response = await billingRepository.UpdateAsync(billing);
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
    public async Task<ResponseApi<SellerRepresentative>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<SellerRepresentative> billing = await billingRepository.DeleteAsync(id);
            if(!billing.IsSuccess) return new(null, 400, billing.Message);
            return billing;
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion 
}
}