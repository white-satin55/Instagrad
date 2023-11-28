using System.Collections;
using System.Drawing;
using System.Net.Mime;

using static System.Net.Mime.MediaTypeNames;

namespace Instagrad.Domain;

public class User
{
    private string _login;
    private string _password;
    private ICollection<Image> _images = new List<Image>();
    private ICollection<User> _friends = new List<User>();
    private ICollection<User> _incomingFriendshipRequests = new List<User>();
    private ICollection<User> _outgoingFriendshipRequests = new List<User>();
    
    /// <summary>
    /// User login used as id
    /// </summary>
    public string Login
    {
        get { return _login; }
    }

    /// <summary>
    /// Readonly list of images uploaded by the user
    /// </summary>
    public IReadOnlyCollection<Image> Images
    {
        get
        {
            return _images.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// User's friends list
    /// </summary>
    public IReadOnlyCollection<User> Friends
    {
        get
        {
            return _friends.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Users who have a sent a friend request
    /// </summary>
    public IReadOnlyCollection<User> IncomingFriendshipRequests
    {
        get { return _incomingFriendshipRequests.ToList().AsReadOnly(); }
    }

    /// <summary>
    /// Users to whom the user sent the request
    /// </summary>
    public IReadOnlyCollection<User> OutgoingFriendshipRequests
    {
        get { return _outgoingFriendshipRequests.ToList().AsReadOnly(); }
    }   

    public User(string login, string password)
    {
        _login = login;
        _password = password;
    }

    /// <summary>
    /// Add user from incoming requests to friends list
    /// </summary>
    /// <param name="user">User being added</param>
    /// <exception cref="InvalidOperationException">Throws if specified user is not in incoming requests</exception>
    public void AcceptFriendshipRequest(User user)
    {
        if (!IncomingFriendshipRequests.Contains(user))
        {
            throw new InvalidOperationException("This user did not send a friendship request!");
        }

        _incomingFriendshipRequests.Remove(user);
        _friends.Add(user);
    }

    public void ReceiveFriendshipRequest(User user)
    {
        if (IncomingFriendshipRequests.Contains(user))
        {
            throw new InvalidOperationException("This user already sent a friendship request!");
        }

        _incomingFriendshipRequests.Add(user);
    }

    public bool CheckCredentials(string login, string password)
    {
        return login.Equals(_login)
            && password.Equals(_password);
    }
}