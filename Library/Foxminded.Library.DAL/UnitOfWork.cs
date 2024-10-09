using Foxminded.Library.DAL.Repositories;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.DAL;

public class UnitOfWork : IUnitOfWork
{
    public IRepository<Book> Books { get; }
    private readonly LibraryDbContext _context;

    public UnitOfWork(LibraryDbContext context, IRepository<Book> bookRepository)
    {
        _context = context ?? throw new ArgumentNullException();
        Books = bookRepository ?? throw new ArgumentNullException();
    }

    public int Complete() => _context.SaveChanges();
}
