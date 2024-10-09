namespace Foxminded.Library.Application.Services.DataSources.FileDataSources.Models;

public class BookFileWithErrorModel : BookFileModel
{
    public required string Error { get; set; }
}
