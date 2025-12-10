using AutoMapper;
using api_slim.src.Models;
using api_slim.src.Shared.DTOs.AccountsReceivable;

namespace api_slim.src.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateAccountsReceivableDTO, AccountsReceivable>().ReverseMap();
            CreateMap<UpdateAccountsReceivableDTO, AccountsReceivable>().ReverseMap();
        }
    }
}