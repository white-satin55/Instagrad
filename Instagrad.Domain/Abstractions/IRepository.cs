namespace Instagrad.Domain.Abstractions;

public interface IRepository<T> : IReadonlyRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}