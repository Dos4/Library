using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Foxminded.Library.DAL.Repositories.IQueryableSupport;

namespace Foxminded.Library.DAL.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    public Type ElementType => typeof(T);
    public Expression Expression { get; }
    public IQueryProvider Provider { get; }

    private readonly LibraryDbContext? _dbContext;
    private readonly DbSet<T>? _targetDbSet;

    public RepositoryBase(LibraryDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException();
        _targetDbSet = _dbContext.Set<T>();
        Expression = Expression.Constant(this);
        Provider = new RepositoryBaseQueryProvider<T>(_targetDbSet);
    }

    public RepositoryBase(IQueryProvider provider, Expression expression)
    {
        Provider = provider;
        Expression = expression;
    }

    public IEnumerator<T> GetEnumerator() =>
                         Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T entity) => _targetDbSet!.Add(entity);

    public void AddRange(IEnumerable<T> entities) => _targetDbSet!.AddRange(entities);

    public void Delete(T entity) => _targetDbSet!.Remove(entity);

    public void Update(T entity) => _targetDbSet!.Entry(entity).State = EntityState.Modified;
}
