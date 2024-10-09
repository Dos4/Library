using AutoMapper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Mappers;

public class BookMapFromBookFileModel : Profile
{
    public BookMapFromBookFileModel()
    {
        CreateMap<BookFileModel, Book>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => new Genre { Name = src.Genre }))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new Author { Name = src.Author }))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => new Publisher { Name = src.Publisher }));
    }
}
