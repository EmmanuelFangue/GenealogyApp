
using AutoMapper;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Domain.Entities;

namespace GenealogyApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<FamilyMember, FamilyMemberDto>().ReverseMap();
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<FamilyLink, FamilyLinkDto>().ReverseMap();
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
        }
    }
}
