namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Models
{
    public class BookFileModel
    {
        public required string Title { get; set; }
        public int Pages { get; set; }
        public required string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Author { get; set; }
        public required string Publisher { get; set; }
    }
}
