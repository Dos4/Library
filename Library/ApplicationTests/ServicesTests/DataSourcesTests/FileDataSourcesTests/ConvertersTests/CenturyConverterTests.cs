using CsvHelper.Configuration;
using CsvHelper;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Converters;
using Moq;

namespace Foxminded.Library.Application.Tests.ServicesTests.DataSourcesTests.FileDataSourcesTests.ConvertersTests;

[TestClass]
public class CenturyConverterTests
{
    private DateTimeConverter? _converter;
    private MemberMapData? _memberMapData;

    [TestInitialize]
    public void TestInitialize()
    {
        _converter = new DateTimeConverter();
        _memberMapData = new MemberMapData(null);
    }

    [TestMethod]
    public void ConvertFromString_WhenCenturyIsGiven_ShouldConvertToCorrectDate()
    {
        var readerRow = new Mock<IReaderRow>();
        var centuryString = "20th century";

        var result = (DateTime)_converter!.ConvertFromString(centuryString, readerRow.Object, _memberMapData!);

        Assert.AreEqual(new DateTime(1901, 1, 1), result);
    }

    [TestMethod]
    public void ConvertFromString_WhenCenturyBCIsGiven_ShouldConvertToCorrectDate()
    {
        var readerRow = new Mock<IReaderRow>();
        var centuryString = "5th century BC";

        var result = (DateTime)_converter!.ConvertFromString(centuryString, readerRow.Object, _memberMapData!);

        Assert.AreEqual(new DateTime(401, 1, 1), result);
    }

    [TestMethod]
    public void ConvertFromString_WhenValidDateStringIsGiven_ShouldConvertToDateTime()
    {
        var readerRow = new Mock<IReaderRow>();
        var validDateString = "2022-12-31";

        var result = (DateTime)_converter!.ConvertFromString(validDateString, readerRow.Object, _memberMapData!);

        Assert.AreEqual(new DateTime(2022, 12, 31), result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertFromString_ShouldThrowFormatException_WhenInvalidDateStringIsGiven()
    {
        var readerRow = new Mock<IReaderRow>();
        var invalidDateString = "invalid date";

        _converter!.ConvertFromString(invalidDateString, readerRow.Object, _memberMapData!);
    }
}
