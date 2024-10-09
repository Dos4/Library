using AutoMapper;
using Foxminded.Library.Application.Services;
using Foxminded.Library.Application.Services.DataSources.FileDataSources;
using Foxminded.Library.Application.Services.DataSources.FileDataSources.Mappers;
using Foxminded.Library.DAL;
using Foxminded.Library.DAL.Repositories;
using Foxminded.Library.Domain.Entities;
using Foxminded.Library.Domain.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Foxminded.Library.Application.ServiceProviderInititalizations;

public static class ServiceCollectionInit
{
    public static IServiceCollection GetServicesTransient(this IServiceCollection service)
    {
        return service
            .AddLogging()
            .AddTransient<LibraryImportService>()
            .AddTransient<BookSearchService>()
            .AddTransient<DuplicateLogService>()
            .AddTransient<BookSearchCriteria>()
            .AddTransient<FileWriterWithDateTime>()
            .AddAutoMapper(typeof(IMapper))
            .AddAutoMapper(typeof(BookMapFromBookFileModel))
            .AddAutoMapper(typeof(BookFileModelMapFromBook))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IRepository<Book>, Repository<Book>>();
    }
}
