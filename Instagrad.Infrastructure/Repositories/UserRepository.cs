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

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>List of all users</returns>
    public ICollection<User> GetAll()
    {
        return _context.Users.ToList().AsReadOnly();
    }

    /// <summary>
    /// Searches for a user by login
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public User GetById(string id)
    {
        return _context.Users.First(u => u.Login.Equals(id));
    }

    /// <summary>
    /// Adds new user
    /// </summary>
    /// <param name="entity">Data of new user</param>
    public void Add(User entity)
    {

        if (_context.Users.Any(u => u.Login.Equals(entity.Login)))
        {
            Update(entity);
        }

        _context.Users.Add(entity);
        _context.SaveChanges();
    }

    /// <summary>
    /// Updates users data
    /// </summary>
    /// <param name="entity">Updated users data</param>
    /// <exception cref="InvalidOperationException">Throws if user with specified id is not found</exception>
    public void Update(User entity)
    {

        if (!_context.Users.Any(u => u.Login.Equals(entity.Login)))
        {
            throw new InvalidOperationException("User with that login not found");
        }

        var updatingUser = _context.Users.First(u => u.Login.Equals(entity.Login));

        Mapper.Register<User, User>();
        entity.Map(updatingUser);

        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes the user from database
    /// </summary>
    /// <param name="entity">Deleting user</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }
}