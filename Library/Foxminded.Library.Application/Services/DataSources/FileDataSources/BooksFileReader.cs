using CsvHelper;
using System.Globalization;
using Foxminded.Library.Domain.Entities;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using AutoMapper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Mappers;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources;

public class BooksFileReader
{
    public string Path => _file.FullName;
    public string FileName => _file.Name;
    private FileInfo _file;
    private IMapper _mapper;

    public BooksFileReader(FileInfo file, IMapper mapper)
    {
        _file = file ?? throw new ArgumentNullException(nameof(file));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public List<Book> Read()
    {
        var books = new List<Book>();
        var reader = new StreamReader(Path);
        var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        csvReader.Context.RegisterClassMap<BookFileModelMap>();
        var records = csvReader.GetRecords<BookFileModel>();
        foreach (var record in records)
        {
            books.Add(_mapper.Map<Book>(record));
        }
        return books;
    }
}
