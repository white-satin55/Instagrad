using System.Data;
using System.Net.Mime;

using Instagrad.Domain;
using Instagrad.Domain.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace Instagrad.Infrastructure.Repositories;

public class ImageRepository : IImageRepository, IDisposable
{
    private readonly InstagradDbContext _context = new();

    public ICollection<Image> GetAll()
    {
        return _context.Images.ToList();
    }

    public Image GetById(string id)
    {        
        return _context.Images
            .AsEnumerable()
            .First(im => im.CheckEncodedId(id));
    }

    public void Add(Image entity)
    {
        _context.Images.Add(entity);        

        _context.SaveChanges();
    }

    public void Update(Image entity)
    {
        throw new NotImplementedException();
    } 

    public Image Delete(Image entity)
    {
        _context.Images.Remove(entity);
        _context.SaveChanges();

        return entity;
    }

    public async Task<ICollection<Image>> GetAllAsync()
    {
        return await _context.Images.ToListAsync();
    }

    public async Task<Image> GetByIdAsync(string id)
    {
        //TODO: make this method async
        return await Task.Run(() => GetById(id));
    }

    public async Task AddAsync(Image entity)
    {
        await _context.Images.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Image entity)
    {
        throw new NotImplementedException();
    }

    public Task<Image> DeleteAsync(Image entity)
    {
        throw new NotImplementedException();
    }    

    public void Dispose()
    {
        _context.Dispose();
    }
}