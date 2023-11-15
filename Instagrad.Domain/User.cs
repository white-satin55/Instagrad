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
    
    public string Login
    {
        get { return _login; }
    }
    public IReadOnlyCollection<Image> Images
    {
        get
        {
            return _images.ToList().AsReadOnly();
        }
    }
    public IReadOnlyCollection<User> Friends
    {
        get
        {
            return _friends.ToList().AsReadOnly();
        }
    }
    public IReadOnlyCollection<User> IncomingFrendshipRequests
    {
        get { return _incomingFriendshipRequests.ToList().AsReadOnly(); }
    }
    public IReadOnlyCollection<User> OutgoingFrendshipRepuests
    {
        get { return _outgoingFriendshipRequests.ToList().AsReadOnly(); }
    }   

    public User(string login, string password)
    {
        _login = login;
        _password = password;
    }

    //public User(string login,
    //    string password,      
    //    ICollection<Image> images = null,
    //    ICollection<User> friends = null,
    //    ICollection<User> frendshipRequests = null)
    //{
    //    _login = login;
    //    _password = password;        
    //    _images = images ?? new List<Image>();
    //    _friends = friends ?? new List<User>();
    //    _incomingFriendshipRequests = frendshipRequests ?? new List<User>();
    //}

    public void AcceptFriendshipRequest(User user)
    {
        if (!IncomingFrendshipRequests.Contains(user))
        {
            throw new InvalidOperationException("This user did not send a friendship request!");
        }

        _incomingFriendshipRequests.Remove(user);
        _friends.Add(user);
    }

    public void SendFriendshipRequest(User user)
    {
        if (user.Friends.Contains(this))
        {
            user._incomingFriendshipRequests.Add(this);
        }
    }

    public void CancelFriendshipRequest(User user)
    {
        if (!_outgoingFriendshipRequests.Contains(user))
        {
            throw new InvalidOperationException("This user didn't send request");
        }

        _outgoingFriendshipRequests.Remove(user);
    }

    public bool CheckCredentials(string login, string password)
    {
        return login.Equals(_login)
            && password.Equals(_password);
    }
}