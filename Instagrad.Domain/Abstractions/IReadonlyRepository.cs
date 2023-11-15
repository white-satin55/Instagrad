using System.Collections;

namespace Instagrad.Domain.Abstractions;

public interface IReadonlyRepository<T>
{
    ICollection<T> GetAll();
}