using AutoMapper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.Application.Tests.ServicesTests.DataSourcesTests.FileDataSourcesTests;

[TestClass]
public class FileWriterWithDateTimeTests
{
    private FileWriterWithDateTime? _fileWriter;
    private List<BookFileModel>? _dataToWrite;
    private string? _fileName;
    private IMapper? _mapper;

    [TestInitialize]
    public void TestInitialize()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Book, BookFileModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name));
        });
        _mapper = config.CreateMapper();

        _fileWriter = new FileWriterWithDateTime(_mapper);
        _dataToWrite = new List<BookFileModel>()
        {
            new BookFileModel()
            {
                Title = "TestTitle",
                Pages = 123,
                ReleaseDate = new DateTime(2000, 1, 1),
                Genre = "TestGenre",
                Author = "TestAuthor",
                Publisher = "TestPublisher",
            }
        };
        _fileName = "testfile.txt";
    }

    [TestMethod]
    public void Write_ShouldCreateFileWithCorrectName()
    {
        var date = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var expectedFileName = $"testfile{date}.txt";

        _fileWriter!.Write(_fileName!, _dataToWrite!);

        Assert.IsTrue(File.Exists(expectedFileName));
    }

    [TestMethod]
    public void Write_ShouldWriteDataToFile()
    {
        var date = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var expectedFileName = $"testfile{date}.txt";

        _fileWriter!.Write(_fileName!, _dataToWrite!);

        var expectedContent = "Title,Pages,Genre,ReleaseDate,Author,Publisher" + Environment.NewLine +
        $"TestTitle,123,TestGenre,01/01/2000 00:00:00,TestAuthor,TestPublisher" + Environment.NewLine;

        var writtenContent = File.ReadAllText(expectedFileName);
        Assert.AreEqual(expectedContent, writtenContent);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowNullException()
    {
        var fileWriterWithDateTime = new FileWriterWithDateTime(null!);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        var date = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var expectedFileName = $"testfile{date}.txt";

        if (File.Exists(expectedFileName))
            File.Delete(expectedFileName);
    }
}

