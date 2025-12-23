using api_slim.src.Handlers;
using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class ServiceModuleService(IServiceModuleRepository serviceModuleRepository, CloudinaryHandler cloudinaryHandler, IMapper _mapper) : IServiceModuleService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<ServiceModule> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> serviceModules = await serviceModuleRepository.GetAllAsync(pagination);
            int count = await serviceModuleRepository.GetCountDocumentsAsync(pagination);
            return new(serviceModules.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> serviceModule = await serviceModuleRepository.GetByIdAggregateAsync(id);
            if(serviceModule.Data is null) return new(null, 404, "Módulo de Serviço não encontrado");
            return new(serviceModule.Data);
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
            PaginationUtil<ServiceModule> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> serviceModules = await serviceModuleRepository.GetAllAsync(pagination);
            return new(serviceModules.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<ServiceModule?>> CreateAsync(CreateServiceModuleDTO request)
    {
        try
        {
            ServiceModule serviceModule = _mapper.Map<ServiceModule>(request);
            ResponseApi<ServiceModule?> response = await serviceModuleRepository.CreateAsync(serviceModule);
            if(response.Data is null) return new(null, 400, "Falha ao criar Módulo de Serviço.");
            
            if(request.Image is not null && response.Data is not null)
            {
                response.Data.Image = await cloudinaryHandler.UploadAttachment("service-module", request.Image);
                await serviceModuleRepository.UpdateAsync(response.Data);
            }

            return new(response.Data, 201, "Módulo de Serviço criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<ServiceModule?>> UpdateAsync(UpdateServiceModuleDTO request)
    {
        try
        {
            ResponseApi<ServiceModule?> serviceModuleResponse = await serviceModuleRepository.GetByIdAsync(request.Id);
            if(serviceModuleResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            ServiceModule serviceModule = _mapper.Map<ServiceModule>(request);
            serviceModule.UpdatedAt = DateTime.UtcNow;
            serviceModule.CreatedAt = serviceModuleResponse.Data.CreatedAt;
            serviceModule.Image = serviceModuleResponse.Data.Image;

            ResponseApi<ServiceModule?> response = await serviceModuleRepository.UpdateAsync(serviceModule);
            if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");

            if(request.Image is not null && response.Data is not null)
            {
                response.Data.Image = await cloudinaryHandler.UploadAttachment("service-module", request.Image);
                await serviceModuleRepository.UpdateAsync(response.Data);
            }

            return new(response.Data, 201, "Atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    public async Task<ResponseApi<ServiceModule?>> UpdateImageAsync(UpdateServiceModuleDTO request)
    {
        try
        {
            ServiceModule serviceModule = new();
            if(!string.IsNullOrEmpty(request.Id))
            {
                ResponseApi<ServiceModule?> serviceModuleResponse = await serviceModuleRepository.GetByIdAsync(request.Id);
                if(serviceModuleResponse.Data is null) return new(null, 404, "Falha ao atualizar");
                
                serviceModuleResponse.Data.UpdatedAt = DateTime.UtcNow;
                serviceModuleResponse.Data.CreatedAt = serviceModuleResponse.Data.CreatedAt;
                serviceModuleResponse.Data.Image = serviceModuleResponse.Data.Image;

                ResponseApi<ServiceModule?> response = await serviceModuleRepository.UpdateAsync(serviceModuleResponse.Data);
                if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");
                
                response.Data!.Image = await cloudinaryHandler.UploadAttachment("service-module", request.Image!);
                await serviceModuleRepository.UpdateAsync(response.Data);
            };

            if(request.Image is not null)
            {
                serviceModule.Image = await cloudinaryHandler.UploadAttachment("service-module", request.Image);
            };

            return new(serviceModule, 200, "Imagem atualizada com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<ServiceModule>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<ServiceModule> serviceModule = await serviceModuleRepository.DeleteAsync(id);
            if(!serviceModule.IsSuccess) return new(null, 400, serviceModule.Message);
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