using AutoMapper;
using telefone_book_api.Models;
using telefone_book_api.Models.Dto;

namespace telefone_book_api.MapperProfiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();
    }
}
