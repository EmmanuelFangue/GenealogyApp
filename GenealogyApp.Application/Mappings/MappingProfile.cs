
using AutoMapper;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Domain.Entities;

namespace GenealogyApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FamilyMember, FamilyMemberDto>().ReverseMap();
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<FamilyLink, FamilyLinkDto>().ReverseMap();
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.ProfileMemberId, opt => opt.MapFrom(src =>
                    src.FamilyMembers.FirstOrDefault(m => m.RelationToUser == "self").MemberId));

        }
    }
}
