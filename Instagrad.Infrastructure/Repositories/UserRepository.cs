using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using ExpressMapper;
using ExpressMapper.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Instagrad.Infrastructure.Repositories;

public class UserRepository : IUserRepository, IDisposable
{
    private readonly InstagradDbContext _context = new();

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

    public User Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _context.Users.FirstAsync(u => u.Login.Equals(id));
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User> DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }    

    public void Dispose()
    {
        _context.Dispose();
    }
}