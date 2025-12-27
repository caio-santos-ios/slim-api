using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class TradingTableService(ITradingTableRepository tradingTableRepository, IMapper _mapper) : ITradingTableService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<TradingTable> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> tradingTables = await tradingTableRepository.GetAllAsync(pagination);
            int count = await tradingTableRepository.GetCountDocumentsAsync(pagination);
            return new(tradingTables.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> tradingTable = await tradingTableRepository.GetByIdAggregateAsync(id);
            if(tradingTable.Data is null) return new(null, 404, "Item não encontrado");
            return new(tradingTable.Data);
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
            PaginationUtil<TradingTable> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> tradingTables = await tradingTableRepository.GetSelectAsync(pagination);
            return new(tradingTables.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<TradingTable?>> CreateAsync(CreateTradingTableDTO request)
    {
        try
        {
            TradingTable tradingTable = _mapper.Map<TradingTable>(request);
            ResponseApi<TradingTable?> response = await tradingTableRepository.CreateAsync(tradingTable);

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
    public async Task<ResponseApi<TradingTable?>> UpdateAsync(UpdateTradingTableDTO request)
    {
        try
        {
            ResponseApi<TradingTable?> tradingTableResponse = await tradingTableRepository.GetByIdAsync(request.Id);
            if(tradingTableResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            TradingTable tradingTable = _mapper.Map<TradingTable>(request);
            tradingTable.UpdatedAt = DateTime.UtcNow;

            ResponseApi<TradingTable?> response = await tradingTableRepository.UpdateAsync(tradingTable);
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
    public async Task<ResponseApi<TradingTable>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<TradingTable> tradingTable = await tradingTableRepository.DeleteAsync(id);
            if(!tradingTable.IsSuccess) return new(null, 400, tradingTable.Message);
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