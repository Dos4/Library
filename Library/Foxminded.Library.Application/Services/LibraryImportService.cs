using Foxminded.Library.Application.Resources;
using Foxminded.Library.Application.Resources.Exceptions;
using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Foxminded.Library.Application.Services;

public class LibraryImportService
{
    private IUnitOfWork _unitOfWork;
    private readonly ILogger<LibraryImportService> _logger;

    public LibraryImportService(IUnitOfWork unitOfWork, ILogger<LibraryImportService> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public void UploadBooks(IEnumerable<Book> books)
    {
        if (books.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(books));

        _logger.LogInformation(TechnicalMessage.StartingImportingText + books.Count());

        var existingGenres = _unitOfWork.Books.AsEnumerable().Select(b => b.Genre).ToList();
        var existingAuthors = _unitOfWork.Books.AsEnumerable().Select(b => b.Author).ToList();
        var existingPublishers = _unitOfWork.Books.AsEnumerable().Select(b => b.Publisher).ToList();

        foreach (var book in books)
        {
            book.Hash = CalculateHash($"{book.Title}-{book.Pages}-{book.ReleaseDate}-{book.GenreId}-" +
                $"{book.AuthorId}-{book.PublisherId}");

            book.Genre.Hash = CalculateHash($"{book.Genre.Name}");
            book.Author.Hash = CalculateHash($"{book.Author.Name}");
            book.Publisher.Hash = CalculateHash($"{book.Publisher.Name}");

            book.Genre = GetOrCreate(book.Genre, existingGenres, g => g.Hash!);
            book.Author = GetOrCreate(book.Author, existingAuthors, a => a.Hash!);
            book.Publisher = GetOrCreate(book.Publisher, existingPublishers, p => p.Hash!);
        }

        VerifyForDuplicates(books);
        _unitOfWork.Books.AddRange(books);
        _unitOfWork.Complete();
        _logger.LogInformation(books.Count() + TechnicalMessage.UploadBooksText);
    }

    private void VerifyForDuplicates(IEnumerable<Book> books)
    {
        foreach (var book in books)
        {
            var isDuplicate = _unitOfWork.Books
                .AsEnumerable()
                .Where(b => b.Hash == book.Hash)
                .Any();

            if (isDuplicate)
                throw new DuplicateBookException();
        }
    }

    private TEntity GetOrCreate<TEntity>(TEntity entity, List<TEntity> existingEntities, Func<TEntity, string> hashSelector)
        where TEntity : class
    {
        var existingEntity = existingEntities.FirstOrDefault(e => hashSelector(e) == hashSelector(entity));
        if (existingEntity != null)
            return existingEntity;

        existingEntities.Add(entity);
        return entity;
    }

    private string CalculateHash(string dataToHash)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(dataToHash);
            var hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
