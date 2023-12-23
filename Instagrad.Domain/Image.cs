using System.ComponentModel.Design;
using System.Net.Mime;
using System.Text;

namespace Instagrad.Domain;

//TODO: try to change Id to filename
//TODO: make Id private field
public class Image
{
    public Guid Id { get; init; }
    public string PublisherLogin { get; init; }
    public string MediaType { get; init; }

    public string GetEncodedId()
    {
        return Convert.ToBase64String(Id.ToByteArray());
    }

    public bool CheckEncodedId(string encodedId)
    {
        return GetEncodedId().Equals(encodedId);
    }
}