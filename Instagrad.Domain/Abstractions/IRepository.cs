namespace Instagrad.Domain.Abstractions;

public interface IRepository<T> : IReadonlyRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    T Delete(T entity);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}