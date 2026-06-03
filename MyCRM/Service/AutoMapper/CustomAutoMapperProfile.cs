using AutoMapper;
using MyCRM.Models;
using MyCRM.Models.DTO;
using MyCRM.Models.Request;

namespace MyCRM.Service.AutoMapper
{
    public class CustomAutoMapperProfile : Profile
    {
        public CustomAutoMapperProfile() 
        {
            base.CreateMap<TUser, TUserDTO>();
            base.CreateMap<TUser, UserCreateEditInfoDTo>()
                .ForMember(dest => dest.UserName, source => source.MapFrom(src => src.Name));
            base.CreateMap<AddUserRequest, TUser>();

            base.CreateMap<TActivity, TActivityDTO>()   
                .ForMember(dest => dest.OwnerName, source => source.MapFrom(src => src.Owner.Name));
            base.CreateMap<TActivityRequest, TActivity>();
            base.CreateMap<TActivity, TActivityDetailDTO>();


            base.CreateMap<TActivityRemark, TActivityRemarkDTO>()
                .ForMember(dest => dest.CreateByName, source => source.MapFrom(src => src.CreateByNavigation.Name))
                .ForMember(dest => dest.EditByName, source => source.MapFrom(src => src.EditByNavigation.Name));

            base.CreateMap<TDicValue, DicValueDTO>();
            base.CreateMap<TActivity, DicValueDTO>()
                .ForMember(dest => dest.Id, source => source.MapFrom(src => src.Id))
                .ForMember(dest => dest.TypeValue, source => source.MapFrom(src => src.Name));

            base.CreateMap<ClueRequest, TClue>();
            base.CreateMap<TClue, ClueDTO>();

            base.CreateMap<ClueRemarkRequest, TClueRemark>();

            base.CreateMap<CustomRequest, TCustomer>();
        }
    }
}

