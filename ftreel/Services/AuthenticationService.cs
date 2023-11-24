using ftreel.DATA;
using ftreel.Entities;
using ftreel.Utils;

namespace ftreel.Services;

public class AuthenticationService
{
    private readonly AppDBContext _dbContext;

    public AuthenticationService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /**
     * Returns the user authenticated, or null if it does not exists in database or password is wrong.
     */
    public User? AuthenticateUser(string username, string password)
    {
        User? userDb = _dbContext.Users
            .FirstOrDefault(u => u.Username == username);

        Console.WriteLine(password);
        Console.WriteLine(PasswordManager.VerifyPassword(password, userDb.Password));
        
        if (userDb == null || !PasswordManager.VerifyPassword(password, userDb.Password))
        {
            return null;
        }
        
        return userDb;
    }

    /**
     * Register a new user that does not exist in database, and authenticate it.
     */
    public User? RegisterUser(string username, string password, IList<string> roles)
    {
        User user = new User()
        {
            Username = username,
            Password = PasswordManager.HashPassword(password),
            Roles = roles
        };

        User? userDb = _dbContext.Users
            .FirstOrDefault(u => u.Username == username);
        
        if (userDb == null)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return AuthenticateUser(username, password);
        }

        return null;
    }
}