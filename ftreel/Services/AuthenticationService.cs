using ftreel.Constants;
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
        var userDb = _dbContext.Users
            .FirstOrDefault(u => u.Mail == username);
        
        if (userDb == null || !PasswordManager.VerifyPassword(password, userDb.Password))
        {
            return null;
        }
        
        return userDb;
    }

    /**
     * Register a new user that does not exist in database, and authenticate it.
     */
    public User? RegisterUser(string username, string password, IList<Roles> roles)
    {
        IList<string> rolesStr = roles.Select(role => role.ToString()).ToList();

        var user = new User()
        {
            Mail = username,
            Password = PasswordManager.HashPassword(password),
            Roles = rolesStr
        };

        var userDb = _dbContext.Users
            .FirstOrDefault(u => u.Mail == username);

        if (userDb != null) return null;
        
        _dbContext.Add(user);
        _dbContext.SaveChanges();
        return AuthenticateUser(username, password);
    }
}