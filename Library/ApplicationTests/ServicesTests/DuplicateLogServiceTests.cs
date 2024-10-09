using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Foxminded.Library.Application.Services;
using Foxminded.Library.DAL.Repositories;
using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Foxminded.Library.Application.Tests.ServicesTests;

[TestClass]
public class DuplicateLogServiceTests
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private Mock<IRepository<Book>>? _repositoryMock;
    private FileWriterWithDateTime? _writer;
    private Mock<ILogger<DuplicateLogService>>? _loggerMock;
    private Mock<IMapper>? _mapperMock;
    private DuplicateLogService? _duplicateLogService;
    private string? _fileName;

    [TestInitialize]
    public void TestInitialize()
    {
        var existingBooks = new List<Book>
            {
                new Book()
                {
                    Title = "Duplicate Book",
                    Pages = 123,
                    ReleaseDate = new DateTime(2000, 1, 1),
                    Author = new Author { Name = "Author" },
                    Genre = new Genre { Name = "Genre" },
                    Publisher = new Publisher { Name = "Publisher" }
                }
            }.AsQueryable();

        _repositoryMock = new Mock<IRepository<Book>>();
        _repositoryMock.Setup(b => b.Provider).Returns(existingBooks.Provider);
        _repositoryMock.Setup(b => b.Expression).Returns(existingBooks.Expression);
        _repositoryMock.Setup(b => b.ElementType).Returns(existingBooks.ElementType);
        _repositoryMock.Setup(b => b.GetEnumerator()).Returns(existingBooks.GetEnumerator());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(uow => uow.Books).Returns(_repositoryMock.Object);

        _mapperMock = new Mock<IMapper>();
        _writer = new FileWriterWithDateTime(_mapperMock.Object);
        _loggerMock = new Mock<ILogger<DuplicateLogService>>();

        _duplicateLogService = new DuplicateLogService(_unitOfWorkMock.Object, _loggerMock.Object, _writer);
        _fileName = "testLog.csv";
    }

    [TestMethod]
    public void CreateLogFileForDuplicateError_WhenDuplicateBooksExist_ShouldLogDuplicates()
    {
        var books = new List<Book>
            {
                new Book() 
                { 
                    Title = "Duplicate Book", 
                    Hash = "123",
                    Author = new Author { Name = "Author" }, 
                    Genre = new Genre { Name = "Genre" }, 
                    Publisher = new Publisher { Name = "Publisher" } 
                }
            };
        var date = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var fileName = Path.GetFileNameWithoutExtension(_fileName);
        var extension = Path.GetExtension(_fileName);
        var expectedFileName = $"{fileName}_result{date}{extension}";

        _duplicateLogService!.CreateLogFileForDuplicateError(_fileName!, books);

        Assert.IsTrue(File.Exists(expectedFileName));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateLogFileForDuplicateError_WhenBooksAreNull_ShouldThrowException()
    {
        _duplicateLogService!.CreateLogFileForDuplicateError(_fileName!, null!);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        var date = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var fileName = Path.GetFileNameWithoutExtension(_fileName);
        var extension = Path.GetExtension(_fileName);
        var expectedFileName = $"{fileName}_result{date}{extension}";

        if (File.Exists(expectedFileName))
            File.Delete(expectedFileName);
    }
}
