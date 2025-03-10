using AutoMapper;
using Ticketing.QueryAPI.Models;

namespace Ticketing.QueryAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 將 Ticket 轉換成 TicketDto
            CreateMap<Ticket, TicketDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MssqlId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price)); 
            
            // 反向轉換
            CreateMap<TicketDTO, Ticket>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  // 讓 MongoDB 自動生成 Id
                .ForMember(dest => dest.MssqlId, opt => opt.MapFrom(src => src.Id))  // 把 DTO 的 Id 存到 MssqlId
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
