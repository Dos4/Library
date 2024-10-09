using AutoMapper;
using CsvHelper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.Domain.Entities;
using System.Globalization;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources;

public class FileWriterWithDateTime
{
    private IMapper _mapper;

    public FileWriterWithDateTime(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void Write<TModel>(string fileName, IEnumerable<TModel> books)
    {
        using (var writer = new StreamWriter(GetFileNameWithDate(fileName)))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<TModel>();
            csv.NextRecord();
            csv.WriteRecords(_mapper.Map<IEnumerable<TModel>>(books));
        }
    }

    private string GetFileNameWithDate(string fileName)
    {
        var absoluteInputFilePath = Path.GetFullPath(fileName);

        var directory = Path.GetDirectoryName(absoluteInputFilePath);
        var originalFileName = Path.GetFileNameWithoutExtension(absoluteInputFilePath);
        var extension = Path.GetExtension(absoluteInputFilePath);
        return Path.Combine(directory!, $"{originalFileName}{DateTime.Now:yyyyMMdd-HHmmss}{extension}");
    }

}
