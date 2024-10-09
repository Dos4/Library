using AutoMapper;
using Foxminded.Library.Application.Resources;
using Foxminded.Library.Application.Resources.Exceptions;
using Foxminded.Library.Application.ServiceProviderInititalizations;
using Foxminded.Library.Application.Services;
using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Foxminded.Library.Application;

public class LibrarianApp
{
    private IServiceProvider _serviceProvider;
    private IEnumerable<Book>? _books;
    private FileInfo? _file;
    private IMapper _mapper;
    private BooksFileReader? _reader;
    private LibraryImportService _importService;
    private FileWriterWithDateTime _writer;
    private BookSearchService _bookSearchService;
    private readonly ILogger<LibrarianApp> _logger;

    public LibrarianApp(string[] args)
    {
        _serviceProvider = new ServiceCollection().GetServiceProvider();

        _mapper = _serviceProvider.GetRequiredService<IMapper>();
        _importService = _serviceProvider.GetRequiredService<LibraryImportService>();
        _writer = _serviceProvider.GetRequiredService<FileWriterWithDateTime>();
        _bookSearchService = _serviceProvider.GetRequiredService<BookSearchService>();
        _logger = _serviceProvider.GetRequiredService<ILogger<LibrarianApp>>();
    }

    public void Execude()
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;
        Console.WriteLine(TechnicalMessage.Greeting);
        try
        {
            var dbContext = _serviceProvider.GetRequiredService<LibraryDbContext>();
            if (dbContext.Database.EnsureCreated() == true)
                _logger.LogInformation(TechnicalMessage.CreatingDatabaseText);
            _logger.LogInformation(TechnicalMessage.ConnectionToDataBaseText);

            Console.WriteLine(TechnicalMessage.AskForInputPathText);
            var usersInput = Console.ReadLine();
            if (usersInput != string.Empty)
            {
                _logger.LogInformation($"{usersInput} - {TechnicalMessage.ReadingPathText}");
                ExecudeUploadService(usersInput!);
            }

            ExecudeBookSearchService();
        }
        catch (ArgumentNullException)
        {
            _logger.LogError(TechnicalMessage.ArgumentNullExceptionText);
        }
        catch (Exception)
        {
            _logger.LogError(TechnicalMessage.ExceptionsText);
        }
    }

    private void ExecudeUploadService(string usersInput)
    {
        try
        {
            _file = new FileInfo(usersInput);
            _reader = new BooksFileReader(_file, _mapper);
            _books = _reader.Read();
            _importService!.UploadBooks(_books);
        }
        catch (DuplicateBookException)
        {
            _logger.LogError(TechnicalMessage.DuplicateException);
            var duplicateService = _serviceProvider.GetRequiredService<DuplicateLogService>();
            duplicateService.CreateLogFileForDuplicateError(usersInput, _books!);
        }
    }

    private void ExecudeBookSearchService()
    {
        _logger.LogInformation(TechnicalMessage.ApplyingFiltersText);
        var filterResults = _bookSearchService.UseFilter();
        if (filterResults.Count() != 0)
        {
            _logger.LogInformation($"{TechnicalMessage.CountFilterResultsText}{filterResults.Count()}");
            Console.WriteLine(TechnicalMessage.FilterResults);
            Console.WriteLine(string.Join(Environment.NewLine, filterResults.Select(b => b.Title).Distinct()));

            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var writer = new FileWriterWithDateTime(mapper);
            var mappedResults = new List<BookFileModel>();
            foreach (var result in filterResults)
            {
                mappedResults.Add(mapper.Map<BookFileModel>(result));
            }

            writer.Write("books.csv", mappedResults);
            _logger.LogInformation(TechnicalMessage.FilterFileCreatedText);
        }
        else
            _logger.LogInformation(TechnicalMessage.EmptyFilterResultsText);
    }
}
