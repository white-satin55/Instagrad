using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using ExpressMapper;
using ExpressMapper.Extensions;

namespace Instagrad.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InstagradDbContext _context = new();

    ~UserRepository()
    {
        _context.Dispose();
    }

    public ICollection<User> GetAll()
    {
        return _context.Users.ToList().AsReadOnly();
    }

    public void Add(User entity)
    {

        if (_context.Users.Any(u => u.Login.Equals(entity.Login)))
        {
            Update(entity);
        }

        _context.Users.Add(entity);
        _context.SaveChanges();
    }

    public void Update(User entity)
    {

        if (!_context.Users.Any(u => u.Login.Equals(entity.Login)))
        {
            throw new InvalidOperationException("Object with that id not found");
        }

        var updatingUser = _context.Users.First(u => u.Login.Equals(entity.Login));

        Mapper.Register<User, User>();
        entity.Map(updatingUser);

        _context.SaveChanges();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }
}