using Castle.Core.Logging;
using Foxminded.Library.Application.Resources.Exceptions;
using Foxminded.Library.Application.Services;
using Foxminded.Library.DAL;
using Foxminded.Library.DAL.Repositories;
using Foxminded.Library.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Foxminded.Library.Application.Tests.ServicesTests;

[TestClass]
public class LibraryImportServiceTests
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private Mock<IRepository<Book>>? _repositoryMock;
    private LibraryImportService? _uploadService;
    private Mock<ILogger<LibraryImportService>>? _loggerMock;
    private IQueryable<Book>? _existingBooks;

    [TestInitialize]
    public void TestInitialize()
    {
        _existingBooks = new List<Book>
        {
            new Book
            {
                Title = "new Book",
                Author = new Author { Name = "Author" },
                Genre = new Genre { Name = "Genre" },
                Publisher = new Publisher {Name = "Publisher"},
            },
        }.AsQueryable();

        _repositoryMock = new Mock<IRepository<Book>>();
        _repositoryMock.Setup(b => b.Provider).Returns(_existingBooks.Provider);
        _repositoryMock.Setup(b => b.Expression).Returns(_existingBooks.Expression);
        _repositoryMock.Setup(b => b.ElementType).Returns(_existingBooks.ElementType);
        _repositoryMock.Setup(b => b.GetEnumerator()).Returns(_existingBooks.GetEnumerator());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(uow => uow.Books).Returns(_repositoryMock.Object);

        _loggerMock = new Mock<ILogger<LibraryImportService>>();

        _uploadService = new LibraryImportService(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public void UploadBooks_WhenNoDuplicates_ShouldAddBooksToDatabase()
    {
        var books = new List<Book>
        {
            new Book 
            { 
                Title = "Book 1",
                Author = new Author { Name = "Author" },
                Genre = new Genre { Name = "Genre" },
                Publisher = new Publisher {Name = "Publisher"}
            },

            new Book
            {
                Title = "Book 2",
                Author = new Author { Name = "Author" },
                Genre = new Genre { Name = "Genre" },
                Publisher = new Publisher {Name = "Publisher"}
            },
        };

        _uploadService!.UploadBooks(books);

        _repositoryMock!.Verify(r => r.AddRange(It.Is<IEnumerable<Book>>(b => b.Count() == 2)), Times.Once);
        _unitOfWorkMock!.Verify(u => u.Complete(), Times.Once);
    }

[TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UploadBooks_WhenListIsNull_ShouldThrowNullException()
    {
        _uploadService!.UploadBooks(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UploadBooks_WhenListIsEmpty_ShouldThrowNullException()
    {
        _uploadService!.UploadBooks(new List<Book>());
    }
}
