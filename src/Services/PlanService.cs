using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class PlanService(IPlanRepository planRepository, IMapper _mapper) : IPlanService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<Plan> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> plans = await planRepository.GetAllAsync(pagination);
            int count = await planRepository.GetCountDocumentsAsync(pagination);
            return new(plans.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> plan = await planRepository.GetByIdAggregateAsync(id);
            if(plan.Data is null) return new(null, 404, "Plano não encontrado");
            return new(plan.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Plan?>> CreateAsync(CreatePlanDTO request)
    {
        try
        {
            Plan plan = _mapper.Map<Plan>(request);
            ResponseApi<Plan?> response = await planRepository.CreateAsync(plan);

            if(response.Data is null) return new(null, 400, "Falha ao criar plano.");
            return new(response.Data, 201, "Plano criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Plan?>> UpdateAsync(UpdatePlanDTO request)
    {
        try
        {
            ResponseApi<Plan?> planResponse = await planRepository.GetByIdAsync(request.Id);
            if(planResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            Plan plan = _mapper.Map<Plan>(request);
            plan.UpdatedAt = DateTime.UtcNow;

            ResponseApi<Plan?> response = await planRepository.UpdateAsync(plan);
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
    public async Task<ResponseApi<Plan>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<Plan> plan = await planRepository.DeleteAsync(id);
            if(!plan.IsSuccess) return new(null, 400, plan.Message);
            return plan;
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion 
}
}