using System.Net.Mime;

namespace Instagrad.Domain;

//TODO: try to change Id to filename
public class Image
{
    public Guid Id { get; init; }
    public string PublisherLogin { get; init; }
    public string MediaType { get; init; }
}