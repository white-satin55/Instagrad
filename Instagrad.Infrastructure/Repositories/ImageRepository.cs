using System.Data;
using System.Net.Mime;
using Instagrad.Domain;
using Instagrad.Domain.Abstractions;

namespace Instagrad.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly InstagradDbContext _context = new();

    ~ImageRepository() 
    {
        _context.Dispose();
    }

    public ICollection<Image> GetAll()
    {
        return _context.Images.ToList();
    }

    public void Add(Image entity)
    {
        _context.Images.Add(entity);
        //TODO: add image to user repo

        _context.SaveChanges();
    }

    public void Update(Image entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Image entity)
    {
        _context.Images.Remove(entity);
        _context.SaveChanges();
    }
}