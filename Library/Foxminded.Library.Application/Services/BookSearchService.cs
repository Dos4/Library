using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Entities;
using Foxminded.Library.Domain.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Foxminded.Library.Application.Services;

public class BookSearchService 
{
    private readonly BookSearchCriteria _filter;
    private readonly IUnitOfWork _unitOfWork;
    private IEnumerable<Book> _query;

    public BookSearchService(IUnitOfWork unitOfWork, IOptions<BookSearchCriteria> filterOptions)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _unitOfWork = unitOfWork;
        _filter = filterOptions.Value;
        _query = _unitOfWork.Books.AsNoTracking();
    }

    public IEnumerable<Book> UseFilter()
    {
        ApplyFilters();
        return _query;
    }

    private void ApplyFilters()
    {
        if (!string.IsNullOrEmpty(_filter.Title))
            _query = _query.Where(b => b.Title.Contains(_filter.Title));

        if (!string.IsNullOrEmpty(_filter.Genre))
            _query = _query.Where(b => b.Genre.Name.Contains(_filter.Genre));

        if (!string.IsNullOrEmpty(_filter.Author))
            _query = _query.Where(b => b.Author.Name.Contains(_filter.Author));

        if (!string.IsNullOrEmpty(_filter.Publisher))
            _query = _query.Where(b => b.Publisher.Name.Contains(_filter.Publisher));

        if (_filter.MoreThanPages.HasValue && _filter.MoreThanPages > 0)
            _query = _query.Where(b => b.Pages > _filter.MoreThanPages);

        if (_filter.LessThanPages.HasValue && _filter.LessThanPages > 0)
            _query = _query.Where(b => b.Pages < _filter.LessThanPages);

        if (_filter.PublishedBefore.HasValue)
            _query = _query.Where(b => b.ReleaseDate < _filter.PublishedBefore);

        if (_filter.PublishedAfter.HasValue)
            _query = _query.Where(b => b.ReleaseDate > _filter.PublishedAfter);
    }
}
