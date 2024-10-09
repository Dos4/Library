using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;

namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Converters;

public class DateTimeConverter : DefaultTypeConverter
{
    private static readonly Regex _century = new Regex(@"(?<number>\d+)(?<suffix>st|nd|rd|th)?\s*century\s*(?<era>BC)?");

    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        var match = _century.Match(text!.ToLower());
        if (!match.Success)
            return Convert.ToDateTime(text);

        int centuryNumber = int.Parse(match.Groups["number"].Value);
        bool isBC = match.Groups["era"].Success;

        int year = (centuryNumber - 1) * 100 + 1;

        if (isBC == true)
            year = -year + 1;

        return new DateTime(year, 1, 1);
    }
}
