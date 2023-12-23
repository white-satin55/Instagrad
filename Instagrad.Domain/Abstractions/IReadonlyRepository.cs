using System.Collections;

namespace Instagrad.Domain.Abstractions;

public interface IReadonlyRepository<T>
{
    ICollection<T> GetAll();
    T GetById(string id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
}