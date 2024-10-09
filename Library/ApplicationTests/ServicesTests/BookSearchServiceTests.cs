using Foxminded.Library.Application.Services;
using Foxminded.Library.Domain.Filters;
using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Moq;
using Microsoft.Extensions.Options;
using Foxminded.Library.DAL.Repositories;

namespace Foxminded.Library.Application.Tests.ServicesTests;

[TestClass]
public class BookSearchServiceTests
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private Mock<IRepository<Book>>? _repositoryMock;
    private Mock<IOptions<BookSearchCriteria>>? _filterOptionsMock;
    private IQueryable<Book>? _bookList;

    [TestInitialize]
    public void Setup()
    {
        _bookList = GetTestBooksForFilter().AsQueryable();

        _repositoryMock = new Mock<IRepository<Book>>();
        _repositoryMock.Setup(b => b.Provider).Returns(_bookList.Provider);
        _repositoryMock.Setup(b => b.Expression).Returns(_bookList.Expression);
        _repositoryMock.Setup(b => b.ElementType).Returns(_bookList.ElementType);
        _repositoryMock.Setup(b => b.GetEnumerator()).Returns(_bookList.GetEnumerator());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(uow => uow.Books).Returns(_repositoryMock.Object);

        _filterOptionsMock = new Mock<IOptions<BookSearchCriteria>>();
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByTitle()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { Title = "1984" });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(b => b.Title.Contains("1984")));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByGenre()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { Genre = "Satire" });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(b => b.Genre.Name.Contains("Satire")));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByAuthor()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { Author = "George Orwell" });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(b => b.Author.Name.Contains("George Orwell")));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByPublisher()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { Publisher = "Secker & Warburg" });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(b => b.Publisher.Name.Contains("Secker & Warburg")));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByMoreThanPages()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { MoreThanPages = 200 });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(b => b.Pages > 200));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByLessThanPages()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { LessThanPages = 200 });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(b => b.Pages < 200));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByPublishedBefore()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { PublishedBefore = new DateTime(1947, 1, 1) });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(b => b.ReleaseDate < new DateTime(1947, 1, 1)));
    }

    [TestMethod]
    public void ApplyFilters_ShouldFilterByPublishedAfter()
    {
        _filterOptionsMock!.Setup(f => f.Value).Returns(new BookSearchCriteria { PublishedAfter = new DateTime(1947, 1, 1) });
        var filterService = new BookSearchService(_unitOfWorkMock!.Object, _filterOptionsMock.Object);

        var result = filterService.UseFilter();

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(b => b.ReleaseDate > new DateTime(1947, 1, 1)));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowNullException()
    {
        var filter = new BookSearchService(null!, _filterOptionsMock!.Object);
    }

    private List<Book> GetTestBooksForFilter()
    {
        return new List<Book>()
        {
            new Book()
                {
                    Title = "1984",
                    Genre = new Genre { Name = "Science Fiction" },
                    Author = new Author { Name = "George Orwell" },
                    Publisher = new Publisher { Name = "Secker & Warburg" },
                    Pages = 328,
                    ReleaseDate = new DateTime(1949, 06, 08)
                },
                new Book()
                {
                    Title = "Animal Farm",
                    Genre = new Genre { Name = "Satire" },
                    Author = new Author { Name = "George Orwell" },
                    Publisher = new Publisher { Name = "Secker & Warburg" },
                    Pages = 122,
                    ReleaseDate = new DateTime(1945, 08, 17)
                },
                new Book()
                {
                    Title = "Dracula",
                    Genre = new Genre { Name = "Horror" },
                    Author = new Author { Name = "Bram Stoker" },
                    Publisher = new Publisher { Name = "Archibald Constable and Company" },
                    Pages = 418,
                    ReleaseDate = new DateTime(1897, 05, 26)
                },
        };
    }
}
