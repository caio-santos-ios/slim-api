using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class HistoricService(IHistoricRepository repository, IMapper _mapper) : IHistoricService
    {
        #region READ
        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<Historic> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> historics = await repository.GetAllAsync(pagination);
                int count = await repository.GetCountDocumentsAsync(pagination);
                return new(historics.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        
        #region CREATE
        public async Task<ResponseApi<Historic?>> CreateAsync(CreateHistoricDTO request)
        {
            try
            {
                Historic historic = _mapper.Map<Historic>(request);
                ResponseApi<Historic?> response = await repository.CreateAsync(historic);

                if(response.Data is null) return new(null, 400, "Falha ao criar Historico.");
                return new(response.Data, 201, "Historico criado com sucesso.");
            }
            catch
            { 
                return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
            }
        }
        #endregion
    }
}