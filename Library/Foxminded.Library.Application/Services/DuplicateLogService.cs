using Foxminded.Library.Application.Resources;
using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Foxminded.Library.Application.Services
{
    public class DuplicateLogService
    {
        private FileWriterWithDateTime _writer;
        private ILogger<DuplicateLogService> _logger;
        private IUnitOfWork _unitOfWork;
        private IEnumerable<Book> _query;

        public DuplicateLogService(IUnitOfWork unitOfWork, ILogger<DuplicateLogService> logger,
            FileWriterWithDateTime writer)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(writer);

            _writer = writer;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _query = _unitOfWork.Books.AsNoTracking();
        }
        public void CreateLogFileForDuplicateError(string originalFilePath, IEnumerable<Book> books)
        {
            ArgumentNullException.ThrowIfNull(books);

            var logFilePath = GetLogFilePath(originalFilePath);

            var booksWithErrors = CreateBookModelWithError(books);

            _writer.Write(logFilePath, booksWithErrors);

            _logger.LogInformation(TechnicalMessage.CreatedLogForDuplicateBooksText);
        }

        private List<BookFileWithErrorModel> CreateBookModelWithError(IEnumerable<Book> books)
        {
            var booksWithErrors = new List<BookFileWithErrorModel>();
            foreach (var book in books)
            {
                var isDuplicate = _query.Any(b => b.Hash == book.Hash);

                booksWithErrors.Add(new BookFileWithErrorModel()
                {
                    Title = book.Title,
                    Genre = book.Genre.Name,
                    Author = book.Author.Name,
                    Publisher = book.Publisher.Name,
                    Pages = book.Pages,
                    ReleaseDate = book.ReleaseDate,
                    Error = isDuplicate ? TechnicalMessage.ErrorDuplicateTextForFile : string.Empty,
                });
            }

            return booksWithErrors;
        }

        private string GetLogFilePath(string originalFilePath)
        {
            var absoluteInputFilePath = Path.GetFullPath(originalFilePath);

            var directory = Path.GetDirectoryName(absoluteInputFilePath);
            var originalFileName = Path.GetFileNameWithoutExtension(absoluteInputFilePath);
            var extension = Path.GetExtension(absoluteInputFilePath);
            return Path.Combine(directory!, $"{originalFileName}_result{extension}");
        }
    }
}
