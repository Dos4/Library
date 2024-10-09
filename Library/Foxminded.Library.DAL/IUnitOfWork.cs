using Foxminded.Library.DAL.Repositories;
using Foxminded.Library.Domain.Entities;

namespace Foxminded.Library.DAL;

public interface IUnitOfWork
{
    public IRepository<Book> Books { get; }
    public int Complete();
}
