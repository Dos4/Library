using AutoMapper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Mappers;

public class BookFileModelMapFromBook : Profile
{
    public BookFileModelMapFromBook()
    {
        CreateMap<Book, BookFileModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name));
    }
}
