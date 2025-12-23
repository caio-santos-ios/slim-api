using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class ProcedureService(IProcedureRepository procedureRepository, IMapper _mapper) : IProcedureService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<Procedure> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> procedures = await procedureRepository.GetAllAsync(pagination);
            int count = await procedureRepository.GetCountDocumentsAsync(pagination);
            return new(procedures.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> procedure = await procedureRepository.GetByIdAggregateAsync(id);
            if(procedure.Data is null) return new(null, 404, "Procedimento não encontrado");
            return new(procedure.Data);
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
            PaginationUtil<Procedure> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> procedure = await procedureRepository.GetAllAsync(pagination);
            return new(procedure.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Procedure?>> CreateAsync(CreateProcedureDTO request)
    {
        try
        {
            Procedure procedure = _mapper.Map<Procedure>(request);
            
            ResponseApi<long> code = await procedureRepository.GetNextCodeAsync();
            procedure.Code = code.Data.ToString().PadLeft(6, '0');
            ResponseApi<Procedure?> response = await procedureRepository.CreateAsync(procedure);

            if(response.Data is null) return new(null, 400, "Falha ao criar Procedimento.");
            return new(response.Data, 201, "Procedimento criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Procedure?>> UpdateAsync(UpdateProcedureDTO request)
    {
        try
        {
            ResponseApi<Procedure?> procedureResponse = await procedureRepository.GetByIdAsync(request.Id);
            if(procedureResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            Procedure procedure = _mapper.Map<Procedure>(request);
            procedure.UpdatedAt = DateTime.UtcNow;

            ResponseApi<Procedure?> response = await procedureRepository.UpdateAsync(procedure);
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
    public async Task<ResponseApi<Procedure>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<Procedure> procedure = await procedureRepository.DeleteAsync(id);
            if(!procedure.IsSuccess) return new(null, 400, procedure.Message);
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