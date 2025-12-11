using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class GenericTableService(IGenericTableRepository genericTableRepository, IMapper _mapper) : IGenericTableService
    {
        #region READ
        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<GenericTable> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> genericTables = await genericTableRepository.GetAllAsync(pagination);
                int count = await genericTableRepository.GetCountDocumentsAsync(pagination);
                return new(genericTables.Data, count, pagination.PageNumber, pagination.PageSize);
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
                ResponseApi<dynamic?> genericTable = await genericTableRepository.GetByIdAggregateAsync(id);
                if(genericTable.Data is null) return new(null, 404, "Usuário não encontrado");
                return new(genericTable.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        #region CREATE
        public async Task<ResponseApi<GenericTable?>> CreateAsync(CreateGenericTableDTO request)
        {
            try
            {
                GenericTable genericTable = _mapper.Map<GenericTable>(request);
                ResponseApi<GenericTable?> response = await genericTableRepository.CreateAsync(genericTable);

                if(response.Data is null) return new(null, 400, "Falha ao criar conta.");
                return new(response.Data, 201, "Tabela Genérica criada com sucesso.");
            }
            catch
            {                
                return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
            }
        }
        #endregion
        #region UPDATE
        public async Task<ResponseApi<GenericTable?>> UpdateAsync(UpdateGenericTableDTO request)
        {
            try
            {
                ResponseApi<GenericTable?> genericTable = await genericTableRepository.GetByIdAsync(request.Id);
                if(genericTable.Data is null) return new(null, 404, "Falha ao atualizar");
                
                genericTable.Data = _mapper.Map<GenericTable>(request);
                genericTable.Data.UpdatedAt = DateTime.UtcNow;

                ResponseApi<GenericTable?> response = await genericTableRepository.UpdateAsync(genericTable.Data);
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
        public async Task<ResponseApi<GenericTable>> DeleteAsync(string genericTableId)
        {
            try
            {
                ResponseApi<GenericTable> genericTable = await genericTableRepository.DeleteAsync(genericTableId);
                if(!genericTable.IsSuccess) return new(null, 400, genericTable.Message);
                return genericTable;
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion        
    }
}