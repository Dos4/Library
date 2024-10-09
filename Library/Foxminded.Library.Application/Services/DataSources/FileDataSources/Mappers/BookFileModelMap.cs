using CsvHelper.Configuration;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Converters;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Mappers;

public class BookFileModelMap : ClassMap<BookFileModel>
{
    public BookFileModelMap()
    {
        Map(m => m.Title);
        Map(m => m.Pages);
        Map(m => m.Genre);
        Map(m => m.ReleaseDate).TypeConverter<DateTimeConverter>();
        Map(m => m.Author);
        Map(m => m.Publisher);
    }
}
