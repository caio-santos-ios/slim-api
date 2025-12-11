using AutoMapper;
using api_slim.src.Models;
using api_slim.src.Shared.DTOs.AccountsReceivable;
using api_slim.src.Shared.DTOs;

namespace api_slim.src.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region MASTER
            CreateMap<CreateGenericTableDTO, GenericTable>().ReverseMap();
            CreateMap<UpdateGenericTableDTO, GenericTable>().ReverseMap();
            #endregion

            CreateMap<CreateAccountsReceivableDTO, AccountsReceivable>().ReverseMap();
            CreateMap<UpdateAccountsReceivableDTO, AccountsReceivable>().ReverseMap();
           
        }
    }
}