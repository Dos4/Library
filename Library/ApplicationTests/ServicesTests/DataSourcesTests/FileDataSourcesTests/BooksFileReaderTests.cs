using CsvHelper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Moq;
using System.Globalization;
using AutoMapper;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.Application.Tests.ServicesTests.DataSourcesTests.FileDataSourcesTests;

[TestClass]
public class BooksFileReaderTests
{
    private Mock<StreamReader>? _mockStreamReader;
    private Mock<CsvReader>? _mockCsvReader;
    private List<BookFileModel>? _bookFileModels;
    private BooksFileReader? _booksFileReader;
    private IMapper? _mapper;

    [TestInitialize]
    public void TestInitialize()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BookFileModel, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => new Genre { Name = src.Genre }))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new Author { Name = src.Author }))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => new Publisher { Name = src.Publisher }));
        });
        _mapper = config.CreateMapper();

        _mockStreamReader = new Mock<StreamReader>("TestsFiles\\test.csv");
        _mockCsvReader = new Mock<CsvReader>(_mockStreamReader.Object, CultureInfo.InvariantCulture);
        _booksFileReader = new BooksFileReader(new FileInfo("TestsFiles\\test.csv"), _mapper);

        _bookFileModels = new List<BookFileModel>
            {
                new BookFileModel()
                {
                    Title = "Test Title",
                    Pages = 123,
                    Genre = "Test Genre",
                    Author = "Test Author",
                    Publisher = "Test Publisher",
                    ReleaseDate = new DateTime(2000, 1, 1)
                },
        };
    }

    [TestMethod]
    public void Read_WhenFileIsValid_ShouldReturnListOfBooks()
    {
        var books = _booksFileReader!.Read();

        Assert.AreEqual(1, books.Count);
        Assert.AreEqual(_bookFileModels![0].Title, books[0].Title);
        Assert.AreEqual(_bookFileModels[0].Pages, books[0].Pages);
        Assert.AreEqual(_bookFileModels[0].Genre, books[0].Genre.Name);
        Assert.AreEqual(_bookFileModels[0].Author, books[0].Author.Name);
        Assert.AreEqual(_bookFileModels[0].Publisher, books[0].Publisher.Name);
        Assert.AreEqual(_bookFileModels[0].ReleaseDate, books[0].ReleaseDate);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_WhenFileIsNull_ShouldThrowArgumentNullException()
    {
        var booksFileReader = new BooksFileReader(null!, null!);
    }
}
