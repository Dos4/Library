namespace Foxminded.Library.DAL.Repositories;

public interface IRepository<TEntity> : IOrderedQueryable<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void Update(TEntity entity);
}
