namespace Instagrad.WebApi.Model;

public class ImageDto : IDisposable
{
    public Stream Content { get; init; }

    public void Dispose()
    {
        Content.Dispose();
    }
}
