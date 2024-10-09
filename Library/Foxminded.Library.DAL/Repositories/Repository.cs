namespace Foxminded.Library.DAL.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(LibraryDbContext dbContext) : base(dbContext)
    {
    }
}
