using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class CommissionService(ICommissionRepository commissionRepository, IMapper _mapper) : ICommissionService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<Commission> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> commissions = await commissionRepository.GetAllAsync(pagination);
            int count = await commissionRepository.GetCountDocumentsAsync(pagination);
            return new(commissions.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> commission = await commissionRepository.GetByIdAggregateAsync(id);
            if(commission.Data is null) return new(null, 404, "Regra de Comissão não encontrada");
            return new(commission.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Commission?>> CreateAsync(CreateCommissionDTO request)
    {
        try
        {
            Commission commission = _mapper.Map<Commission>(request);
            ResponseApi<Commission?> response = await commissionRepository.CreateAsync(commission);

            if(response.Data is null) return new(null, 400, "Falha ao criar Regra de Comissão.");
            return new(response.Data, 201, "Regra de Comissão criada com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Commission?>> UpdateAsync(UpdateCommissionDTO request)
    {
        try
        {
            ResponseApi<Commission?> commissionResponse = await commissionRepository.GetByIdAsync(request.Id);
            if(commissionResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            Commission commission = _mapper.Map<Commission>(request);
            commission.UpdatedAt = DateTime.UtcNow;

            ResponseApi<Commission?> response = await commissionRepository.UpdateAsync(commission);
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
    public async Task<ResponseApi<Commission>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<Commission> commission = await commissionRepository.DeleteAsync(id);
            if(!commission.IsSuccess) return new(null, 400, commission.Message);
            return new(null, 204, "Exclído com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion 
}
}