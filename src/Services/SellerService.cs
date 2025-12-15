using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using api_slim.src.Shared.Utils;
using AutoMapper;

namespace api_slim.src.Services
{
    public class SellerService(ISellerRepository sellerRepository, IAddressRepository addressRepository, IMapper _mapper) : ISellerService
{
    #region READ
    public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
    {
        try
        {
            PaginationUtil<Seller> pagination = new(request.QueryParams);
            ResponseApi<List<dynamic>> sellers = await sellerRepository.GetAllAsync(pagination);
            int count = await sellerRepository.GetCountDocumentsAsync(pagination);
            return new(sellers.Data, count, pagination.PageNumber, pagination.PageSize);
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
            ResponseApi<dynamic?> seller = await sellerRepository.GetByIdAggregateAsync(id);
            if(seller.Data is null) return new(null, 404, "Vendedor não encontrado");
            return new(seller.Data);
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Seller?>> CreateAsync(CreateSellerDTO request)
    {
        try
        {
            Seller seller = _mapper.Map<Seller>(request);
            ResponseApi<Seller?> response = await sellerRepository.CreateAsync(seller);

            request.Address.Parent = "seller";
            request.Address.ParentId = response.Data!.Id;

            Address address = _mapper.Map<Address>(request.Address);
            ResponseApi<Address?> addressResponse = await addressRepository.CreateAsync(address);
            if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao criar Vendedor.");

            if(response.Data is null) return new(null, 400, "Falha ao criar Vendedor.");
            return new(response.Data, 201, "Vendedor criado com sucesso.");
        }
        catch
        { 
            return new(null, 500, $"Ocorreu um erro inesperado. Por favor, tente novamente mais tarde");
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Seller?>> UpdateAsync(UpdateSellerDTO request)
    {
        try
        {
            ResponseApi<Seller?> sellerResponse = await sellerRepository.GetByIdAsync(request.Id);
            if(sellerResponse.Data is null) return new(null, 404, "Falha ao atualizar");
            
            Seller seller = _mapper.Map<Seller>(request);
            seller.UpdatedAt = DateTime.UtcNow;

            ResponseApi<Seller?> response = await sellerRepository.UpdateAsync(seller);
            if(!response.IsSuccess) return new(null, 400, "Falha ao atualizar");

            ResponseApi<Address?> addressResponse = await addressRepository.UpdateAsync(request.Address);
            if(!addressResponse.IsSuccess) return new(null, 400, "Falha ao atualizar Vendedor.");

            return new(response.Data, 201, "Atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Seller>> DeleteAsync(string id)
    {
        try
        {
            ResponseApi<Seller> seller = await sellerRepository.DeleteAsync(id);
            if(!seller.IsSuccess) return new(null, 400, seller.Message);
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